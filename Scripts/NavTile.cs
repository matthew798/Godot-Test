using Godot;
using System;

[Tool]
public partial class NavTile : MeshInstance3D
{
	private NavigationRegion3D _navRegion;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_navRegion = GetNode<NavigationRegion3D>("NavigationRegion3D");
		_navRegion.Navmesh.CreateFromMesh(this.Mesh);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
