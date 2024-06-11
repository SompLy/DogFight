using Godot;
using System;

public partial class Enemy1Controller : EnemyController
{
	// Grenade Guy
	enum EState
	{
		Idle,
		Walking,
		Grenade
	}
	private EState _state = EState.Walking;
	private bool _isAttackRange;
	private Map _map;
	public override void _Process(double delta)
	{
		base._Process( delta );

		if ( _map == null )
		{
			_map = GetParent().GetNode<Map>( "Map" );
		}
		
		AttackTimer -= delta;

		if ( DistanceToPlayer.X > -100.0f &&
		     DistanceToPlayer.X < 100.0f )
			_isAttackRange = true;
		else
			_isAttackRange = false;
		
		switch ( _state )
		{
			case EState.Idle:
				break;
			case EState.Walking:
				// Walking
				if ( DistanceToPlayer.X > 0 && DistanceToPlayer.X > 50.0f)
					Walk = -1;
				else if ( DistanceToPlayer.X < 0 && DistanceToPlayer.X < -50.0f )
					Walk = 1;
				else
					Walk = 0;
				
				if ( Map.CollisionNormalPoint( Position + new Godot.Vector2( 1, -2 ) ) != Vector2.Zero || 
				     Map.CollisionNormalPoint( Position + new Godot.Vector2( -1, -2 ) ) != Vector2.Zero )
				{
					ShouldJump = true;
				}
				
				// Attack if in range
				if ( _isAttackRange )
					_state = EState.Grenade;
				
				break;
			case EState.Grenade:
				if ( AttackTimer <= 0 )
				{
					ShouldJump = true;
					InstantiateGrenade( ( PlayerController.GlobalPosition - Position ) );
					AttackTimer = 1.0f;
				}
				else
				{
					_state = EState.Walking;
				}
				break;
			default:
				break;
		}
	}
}
