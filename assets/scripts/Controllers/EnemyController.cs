using Godot;
using System;

public partial class EnemyController : Controller
{
	enum EState
	{
		Idle,
		Walking,
		BazookaShort,
		BazookaLong,
		Grenade
	}
	private EState _state = EState.Walking;

	private Vector2			 _distanceToPlayer = Vector2.Zero;
	private PlayerController _playerController;
	
	public override void _Ready()
	{
		base._Ready();

		_playerController = GetNode<PlayerController>( "../Player1" );
	}
	public override void _Process(double delta)
	{
		base._Process( delta );

		switch ( _state )
		{
			case EState.Idle:
				
				break;
			case EState.Walking:
				if ( _distanceToPlayer.X > 0 && _distanceToPlayer.X > 50.0f)
					Walk = -1;
				else if ( _distanceToPlayer.X < 0 && _distanceToPlayer.X < -50.0f )
					Walk = 1;
				else
					Walk = 0;
				
				break;
			case EState.BazookaShort:
				
				break;
			case EState.BazookaLong:
				
				break;
			case EState.Grenade:
				
				break;
			default:
				break;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess( delta );

		_distanceToPlayer = Position - _playerController.Position;
	}
}
