using Godot;
using System;

public partial class CameraTargetsManager : Node
{
	private Player _player;
	private Label _label;
	private MultiTargetDynamicCamera2D _multiTargetDynamicCamera2D;
	public override void _Ready()
	{
		_player = GetNode<Player>( "../Player" );
		_label = GetNode<Label>( "../Label" );
		_multiTargetDynamicCamera2D = GetNode<MultiTargetDynamicCamera2D>( "../MultiTargetDynamicCamera2D" );
		
		_multiTargetDynamicCamera2D.AddTarget( _player.Position );
		_multiTargetDynamicCamera2D.AddTarget( _label.Position );
		
		_multiTargetDynamicCamera2D.LimitLeft = -3000;
		_multiTargetDynamicCamera2D.LimitRight = 3000;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_multiTargetDynamicCamera2D.UpdateTarget( _player.Position, 0 );
		_multiTargetDynamicCamera2D.UpdateTarget( _label.Position, 1 );
	}
}
