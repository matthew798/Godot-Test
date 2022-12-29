using Godot;
using System;

public partial class Character : Node3D
{
	[Export] private NodePath _targetNode;

	private Node3D _target;
	private NavigationAgent3D _navAgent;

	private AnimationPlayer _animPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_target = GetNode<Node3D>(_targetNode);
		_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		_animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_navAgent.TargetLocation = _target.GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        if(_navAgent.IsTargetReachable() && !_navAgent.IsTargetReached()){
			_animPlayer.Play("SlowRun(2)");
			var nextLocation = _navAgent.GetNextLocation();
			var direction = GlobalPosition.DirectionTo(nextLocation);
			LookAt(-nextLocation, Vector3.Up);

			GlobalPosition += direction * new Vector3((float)delta, (float)delta, (float)delta);
		}
    }
}
