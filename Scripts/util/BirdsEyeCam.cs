using Godot;
using System;

public partial class BirdsEyeCam : Camera3D
{

	[Export] private float _speed = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("move_right")){
			Translate(new Vector3((float)((Transform.basis.x.x + delta) * _speed), 0, 0));
		}
		if(Input.IsActionPressed("move_left")){
			Translate(new Vector3(-(float)((Transform.basis.x.x + delta) * _speed), 0, 0));
		}
		if(Input.IsActionPressed("move_forward")){
			var vec = new Vector3(0, 0, -(float)((Transform.basis.z.z + delta) * _speed));
			vec = vec.Rotated(new Vector3(1, 0, 0), -Rotation.x);
			Translate(vec);
		}
		if(Input.IsActionPressed("move_backward")){
			var vec = new Vector3(0, 0, (float)((Transform.basis.z.z + delta) * _speed));
			vec = vec.Rotated(new Vector3(1, 0, 0), -Rotation.x);
			Translate(vec);
		}
		if(Input.IsActionPressed("cam_up")){
			var vec = Vector3.Up * new Vector3(0, _speed, 0);
			vec = vec.Rotated(new Vector3(1, 0, 0), -Rotation.x);
			Translate(vec);
		}
		if(Input.IsActionPressed("cam_down")){
			var vec = Vector3.Up * new Vector3(0, _speed, 0);
			vec = vec.Rotated(new Vector3(1, 0, 0), -Rotation.x);
			Translate(-vec);
		}
    }
}
