using Godot;
using System;

public partial class Enemy2Controller : EnemyController
{
	// Grenade Guy
	enum EState
	{
		Idle,
		Walking,
		BazookaShort,
		BazookaLong
	}
	private EState _state = EState.Walking;
	
	public override void _Process(double delta)
	{
		base._Process( delta );

		switch ( _state )
		{
			case EState.Idle:
				
				break;
			case EState.Walking:
				if ( DistanceToPlayer.X > 0 && DistanceToPlayer.X > 600.0f)
					Walk = -1;
				else if ( DistanceToPlayer.X < 0 && DistanceToPlayer.X < 600.0f )
					Walk = 1;
				else
					Walk = 0;
				
				break;
			case EState.BazookaShort:
				
				break;
			case EState.BazookaLong:
				
				break;
			default:
				break;
		}
	}
}
