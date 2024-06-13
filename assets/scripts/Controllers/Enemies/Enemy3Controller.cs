using Godot;
using System;

public partial class Enemy3Controller : EnemyController
{
	// Örjan den Överlägsne
	enum EState
	{
		Idle,
		Walking,
		BazookaShort,
		BazookaLong,
		Grenade,
		Spin
	}
	private EState _state = EState.Walking;
	private bool _inAttackRangeLong;
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	public override void _Process(double delta)
	{
		base._Process( delta );

		// If in range
		_inAttackRangeLong = Mathf.Abs( DistanceToPlayer.X ) > 200.0f;
		
		switch ( _state )
		{
			case EState.Idle:
				break;
			case EState.Walking:
				if ( DistanceToPlayer.X > 0 && DistanceToPlayer.X > 300.0f)
					Walk = -1;
				else if ( DistanceToPlayer.X < 0 && DistanceToPlayer.X < 300.0f )
					Walk = 1;
				else
					Walk = 0;
				
				if ( Map.CollisionNormalPoint( Position + new Godot.Vector2( 1,  -2 ) ) != Vector2.Zero || 
				     Map.CollisionNormalPoint( Position + new Godot.Vector2( -1, -2 ) ) != Vector2.Zero )
					ShouldJump = true;

				if ( AttackTimer1 >= 0 )
					break;
				
				_state = _inAttackRangeLong ? EState.BazookaLong : EState.BazookaShort;
				
				break;
			case EState.BazookaShort:
				if ( AttackTimer1 <= 0 )
				{
					CurrentWeapon = EWeapon.Bazooka;
					InstantiateBazooka( PlayerController.GlobalPosition - Position * 
						_randomNumberGenerator.RandfRange( 1.0f, 2.5f ), PlayerIndex);
					AttackTimer1 = 0.25f;
				}
				_state = EState.Walking;
				
				break;
			case EState.BazookaLong:
				if ( AttackTimer1 <= 0 )
				{
					CurrentWeapon = EWeapon.Bazooka;
					ShouldJump = true;
					InstantiateBazooka( PlayerController.GlobalPosition - Position * 1.5f, PlayerIndex );
					AttackTimer1 = 0.3f;
				}

				_state = EState.Grenade;
				
				break;
			case EState.Grenade:
				if ( AttackTimer2 <= 0.0f )
				{
					CurrentWeapon = EWeapon.Grenade;
					InstantiateGrenade( PlayerController.GlobalPosition - Position, PlayerIndex );
					ShouldJump   = _randomNumberGenerator.RandiRange( 0, 2 ) == 0;
					AttackTimer1 = _randomNumberGenerator.RandfRange( 0.5f, 1.2f );
					AttackTimer2 = 0.1f;
				}
				_state = EState.Walking;
				
				break;
			case EState.Spin:
				
				break;
			default:
				break;
		}
	}
}