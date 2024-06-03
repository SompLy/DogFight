using Godot;
using System;

public partial class CameraTargetsManager : Node
{
	private PlayerController _player1;
	private PlayerController _player2;
	private MultiTargetDynamicCamera2D _multiTargetDynamicCamera2D;
	public override void _Ready()
	{
		_player1 = GetNode<PlayerController>( "../Player1" );
		_player2 = GetNode<PlayerController>( "../Player2" );
		_multiTargetDynamicCamera2D = GetNode<MultiTargetDynamicCamera2D>( "../MultiTargetDynamicCamera2D" );
		
		_multiTargetDynamicCamera2D.AddTarget( _player1.Position );
		_multiTargetDynamicCamera2D.AddTarget( _player2.Position );
		
		_multiTargetDynamicCamera2D.LimitLeft = -3000;
		_multiTargetDynamicCamera2D.LimitRight = 3000;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print( _multiTargetDynamicCamera2D.GetTargets().Count );
		if ( IsInstanceValid( _player1 ) )
			_multiTargetDynamicCamera2D.UpdateTarget( _player1.Position, 0 );
		// else
		// 	_multiTargetDynamicCamera2D.RemoveTarget( 0 );
		if ( IsInstanceValid( _player2 ) )
			_multiTargetDynamicCamera2D.UpdateTarget( _player2.Position, 1 );
		// else
		// 	_multiTargetDynamicCamera2D.RemoveTarget( 1 );
	}
}
