using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[Tool]
public partial class LevelManager : Node3D
{
	private string _levelFile;
	private Node3D _levelNode;

	[Export]
	public string LevelFile{
		get => _levelFile;
		set{
			_levelFile = value;
			
		}
	}
	//Dictionary of possible blocks name => resource
	private Dictionary<String, PackedScene> _blockList = new Dictionary<String, PackedScene>();
	List<Block> WorkQueue;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadBlocks();
		

		_levelNode = new Node3D() {Name = "Level"};
		AddChildEditor(_levelNode);

		LoadLevel();
	}

	private void LoadLevel(){
		var path = ProjectSettings.GlobalizePath(_levelFile);
		if(!File.Exists(path)){
			throw new FileNotFoundException(path);
		}
			/*
			Level tst = new Level(){
			Name = "Testing Ground",
			Width = 16, Height = 16,
			Blocks = new string[] {
				"rock",
				"rock_floor",
				"dungeon_heart"
			},
			Layout = new Level.LevelLayout(){
				Blocks = new int[][] {
					new int[]{	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 1,
						1, 2, 2, 2, 2, 2, 2, 0, 3, 0, 2, 2, 2, 2, 2, 1,
						1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 2, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
						1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
					}
				}
			}
		};

		var output = JsonConvert.SerializeObject(tst);
		*/

		Level level = JsonConvert.DeserializeObject<Level>(File.ReadAllText(path));

		//Load the blocks we need to build the level in the same order as the level file


		//Start placing blocks!
		PlaceBlocks(level);
	}

	// Loads all the specified blocks from the given path. 
	// If no blocks are specified, they are all loaded.
	private void LoadBlocks(string path = "Blocks"){
		var files = Directory.GetFiles(path, "*.tscn", SearchOption.AllDirectories);

		foreach(String file in files){
			var filename = Path.GetFileNameWithoutExtension(file);
			_blockList[filename] = GD.Load<PackedScene>($"res://{path}/{filename}.tscn");
		}
	}
	private void PlaceBlock(string blockName, Vector2 position, int level = 0){
		MeshInstance3D block = _blockList[blockName].Instantiate<MeshInstance3D>();

		AddChild(block);
		block.GlobalPosition = new Vector3(position.x, 1 * level, position.y);
		
	}

	private void PlaceBlocks(Level level){
		List<PackedScene> blockList = new List<PackedScene>();

		foreach(string blockName in level.Blocks){
			if(!_blockList.ContainsKey(blockName))
				throw new KeyNotFoundException($"Block \"{blockName}\" does not exist.");

			blockList.Add(_blockList[blockName]);
		}

		foreach(int[] floor in level.Layout.Blocks){
			for(int i = 0; i < level.Height; i++){
				for(int j = 0; j < level.Width; j++){
					int blockId = floor[i * level.Height + j];

					if(blockId == 0)
						continue;

					var block = blockList[blockId - 1].Instantiate<MeshInstance3D>();
					block.Position = new Vector3(j, 0, i);

					_levelNode.AddChild(block);
					block.Owner = _levelNode;
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//Hides engine method to enable nodes to be seen in the editor
	private void AddChildEditor(Node node){
		if(Engine.IsEditorHint()){
			node.Owner = GetTree().EditedSceneRoot;
		}

		AddChild(node);
	}
}
