using Godot;

public partial class RockWallBlock : Block{
    public RockWallBlock() : base(BlockType.RockWall){ }

    public bool Selected{ get; set; }

    public override void _Ready()
	{
        base._Ready(); //Must be called for child nodes to be assigned to local variables
		_staticBody.InputEvent += OnToggleSelected;
	}

    private void OnToggleSelected(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx){
        if(@event is InputEventMouseButton button){
				if(button.ButtonIndex == MouseButton.Left && button.Pressed){
                    Selected = !Selected;
					this.SetInstanceShaderParameter("selected", Selected);
				}
			}
    }

}