using Godot;

public partial class PlayerControls : Resource
{
	[Export]
	public int PlayerIndex { get; set; } = 0;
	[Export]
	public string MoveLeft { get; set; } = "p1_left";
	[Export]
	public string MoveRight { get; set; } = "p1_right";
	[Export]
	public string Jump { get; set; } = "p1_up";
	[Export]
	public string Attack { get; set; } = "p1_attack";
	[Export]
	public string AimLeft { get; set; } = "p1_aimLeft";
	[Export]
	public string AimRight { get; set; } = "p1_aimRight";
	[Export]
	public string Switch { get; set; } = "p1_switch";
}
