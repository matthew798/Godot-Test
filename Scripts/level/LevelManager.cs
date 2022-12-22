using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public partial class LevelManager : Node3D
{
	private string _levelFile;
	private Node3D _levelNode;

	[Export]
	public string LevelFile{
		get => _levelFile;
		set{
			_levelFile = value;
			RequestReady();
		}
	}
	//Dictionary of possible blocks. name => resource
	private Dictionary<String, PackedScene> _blockList = new Dictionary<String, PackedScene>();
	List<Block> WorkQueue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadBlocks();
		
		if(_levelNode == null){
			_levelNode = new Node3D() {Name = "Level"};
			AddChild(_levelNode);
			if(Engine.IsEditorHint()){
				_levelNode.Owner = this;
			}
		}

		LoadLevel();
	}

	/// <summary>
	/// Loads the level information from the file at <see>_levelFile</see>
	/// ans places all the blocks
	/// </summary>
	private void LoadLevel(){
		var path = ProjectSettings.GlobalizePath(_levelFile);
		if(!File.Exists(path)){
			throw new FileNotFoundException(path);
		}

		Level level = JsonConvert.DeserializeObject<Level>(File.ReadAllText(path));

		//Start placing blocks!
		PlaceBlocks(level);
	}

	/// <summary>
	/// Loads all the available blocks from the given directory into memory as <see>PackedScene</see>
	/// </summary>
	/// <param name="path">The path containing the block scene files</param>
	private void LoadBlocks(string path = "Prefabs/Blocks"){
		_blockList.Clear();

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

	/// <summary>
	/// Places all the blocks required for the given level
	/// </summary>
	/// <param name="level">The level object</param>
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

					_levelNode.AddChild(block);
					/*if(Engine.IsEditorHint()){
						block.Owner = _levelNode;
					}*/

					block.Position = new Vector3(j, 0, i);
				}
			}
		}
	}
}
