using Godot;
using System;

public enum BlockType{
	RockWall,
	RockFloor,
	DungeonHeart
}

/* 
Base class for all block types.

All blocks have at least:
- One mesh and associated material
- One StaticBody3D and associated CollisionShape3D

In addition, any block that is navigable (I.E. can be walked on) has:
- One NavigationRegion3D and any number of associated meshes
*/
public partial class Block : MeshInstance3D
{
	protected StaticBody3D _staticBody;

	public BlockType Type{ get; private set;}

	public Block(BlockType type){
		Type = type;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_staticBody = GetNode<StaticBody3D>("StaticBody3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
