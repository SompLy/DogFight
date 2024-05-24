using Godot;
using System;

public partial class CameraTargetsManager : Node
{
	private Player _player1;
	private Player _player2;
	private MultiTargetDynamicCamera2D _multiTargetDynamicCamera2D;
	public override void _Ready()
	{
		_player1 = GetNode<Player>( "../Player1" );
		_player2 = GetNode<Player>( "../Player2" );
		_multiTargetDynamicCamera2D = GetNode<MultiTargetDynamicCamera2D>( "../MultiTargetDynamicCamera2D" );
		
		_multiTargetDynamicCamera2D.AddTarget( _player1.Position );
		_multiTargetDynamicCamera2D.AddTarget( _player2.Position );
		
		_multiTargetDynamicCamera2D.LimitLeft = -3000;
		_multiTargetDynamicCamera2D.LimitRight = 3000;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_multiTargetDynamicCamera2D.UpdateTarget( _player1.Position, 0 );
		_multiTargetDynamicCamera2D.UpdateTarget( _player2.Position, 1 );
	}
}
