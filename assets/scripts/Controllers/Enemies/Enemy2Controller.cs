using Godot;
using System;

public partial class Enemy2Controller : EnemyController
{
	// Bångstyriga Bazooka Benny
	enum EState
	{
		Idle,
		Walking,
		BazookaShort,
		BazookaLong
	}
	private EState _state = EState.Walking;
	private bool _inAttackRangeLong;
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	public override void _Ready()
	{
		base._Ready();
		CurrentWeapon = EWeapon.Bazooka;
	}
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
				if ( DistanceToPlayer.X > 0 && DistanceToPlayer.X > 400.0f)
					Walk = -1;
				else if ( DistanceToPlayer.X < 0 && DistanceToPlayer.X < 400.0f )
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
					GD.Print( "short" );
				if ( AttackTimer1 <= 0 )
				{
					InstantiateBazooka( PlayerController.GlobalPosition - Position * 
						_randomNumberGenerator.RandfRange( 1.0f, 2.5f ), PlayerIndex);
					AttackTimer1 = 0.25f;
				}
				_state = EState.Walking;
				
				break;
			case EState.BazookaLong:
					GD.Print( "long" );
				if ( AttackTimer1 <= 0 )
				{
					ShouldJump = true;
					InstantiateBazooka( PlayerController.GlobalPosition - Position * 1.5f, PlayerIndex );
					AttackTimer1 = 1.0f;
				}
				_state = EState.Walking;
				
				break;
			default:
				break;
		}
	}
}
