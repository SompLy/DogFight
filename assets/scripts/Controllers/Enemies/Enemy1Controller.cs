using Godot;
using System;

public partial class Enemy1Controller : EnemyController
{
	// Gränslösa Granat-Göran
	enum EState
	{
		Idle,
		Walking,
		Grenade
	}
	private EState _state = EState.Walking;
	private bool _inAttackRange;
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	
	public override void _Ready()
	{
		base._Ready();
		CurrentWeapon = EWeapon.Grenade;
	}

	public override void _Process(double delta)
	{
		base._Process( delta );

		// If in range
		_inAttackRange = Mathf.Abs( DistanceToPlayer.X ) < 300;
		
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
					ShouldJump = true;
				
				// Attack if in range
				if ( _inAttackRange && AttackTimer1 <= 0 )
					_state = EState.Grenade;
				
				break;
			case EState.Grenade:
					InstantiateGrenade( PlayerController.GlobalPosition - Position, PlayerIndex );
					ShouldJump = _randomNumberGenerator.RandiRange( 0, 2 ) == 0;
					AttackTimer1 = _randomNumberGenerator.RandfRange( 0.5f, 1.2f );
					_state = EState.Walking;
					
				break;
			
			default:
				break;
		}
	}
}
