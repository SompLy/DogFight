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
		BazookaSpin
	}
	private EState _state = EState.Walking;
	private bool _inAttackRangeLong;
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	// Spin
	private float _spinTime;
	private float _spinJumpWeight;
	private float _spinDegrees;
	private Vector2 _spinPosition;
	public Node2D RotationPoint;
	public Node2D DirectionSprite;

	public override void _Ready()
	{
		base._Ready();
		RotationPoint   = GetNode<Node2D>( "RotationPoint" );
		DirectionSprite = GetNode<Sprite2D>( "RotationPoint/DirectionSprite" );
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
				
				// Set state
				_state = _inAttackRangeLong ? EState.BazookaLong : EState.BazookaShort;

				if ( _randomNumberGenerator.RandiRange( 0, 5000 ) == 0 )
				{
					Walk      = 0;
					_state    = EState.BazookaSpin;
					_spinTime = 5.0f;
				}
				
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
			case EState.BazookaSpin:
				InternalVelocity = Vector2.Zero;
				if( _spinJumpWeight < 0.2f )
				{
					GD.Print( _spinJumpWeight );
					_spinJumpWeight += (float)delta * 0.1f;
					Position        =  LerpVector2( Position, new Vector2( Position.X, 300.0f ), _spinJumpWeight);
					_spinPosition   =  Position;
					break;
				}
				
				if ( _spinJumpWeight >= 0.2f && _spinTime > 0.0f)
				{
					Position  =  _spinPosition;
					_spinTime -= (float)delta;
						
					if ( AttackTimer1 <= 0.0f )
					{
						InstantiateBazooka( ( DirectionSprite.GlobalPosition - RotationPoint.GlobalPosition ) *
						                    AttackPowerBazooka * 1.5f, PlayerIndex );
						RotationPoint.RotationDegrees -= 9.0f;
						AttackTimer1                  =  0.05f;
					}
				}
				// Switch state and reset
				if ( _spinTime <= 0.0f )
				{
					_spinJumpWeight = 0.0f;
					_state          = EState.Walking;
				}
				
				break;
			default:
				break;
		}
	}
	private Vector2 LerpVector2( Vector2 start, Vector2 end, float percentage )
	{
		Vector2 tempVec = new Vector2();
		tempVec.X = start.X + ( end.X - start.X ) * percentage;
		tempVec.Y = start.Y + ( end.Y - start.Y ) * percentage;
		
		return tempVec;
	}
}