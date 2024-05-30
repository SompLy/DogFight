using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private GameManager _gameManager;
	
	private CollisionShape2D _collisionShape;
	private Vector2[]        _rectPoints;
	private Map				 _map;

	[Export] public  int		Health				= 3;
	[Export] private float		_attackPowerBazooka	= 40.0f;
	[Export] private float		_attackPowerGrenade	= 7.0f;
			 public  Vector2	InternalVelocity 	= Vector2.Zero;
			 public  bool		IsAirborne		 	= true;
			 private bool		_isSpriteFlipped 	= false;
			 
	 enum EWeapon
	 {
		 Bazooka,
		 Grenade,
		 Molotov
	 }

	 private EWeapon _currentWeapon = EWeapon.Bazooka;
			 
	private const float Gravity = 0.1f;

	[Export] private PlayerControls _controls;
	[Export] private Texture2D		_playerTexture;
			 private Sprite2D		_playerSprite;
			 private Node2D 		_rotationPoint;
			 private Node2D 		_directionSprite;
	
	public override void _Ready()
	{
		_gameManager	 = GetNode<GameManager>		( "/root/GameManager" );
		_map			 = GetNode<Map>				( "../Map" );
		_collisionShape  = GetNode<CollisionShape2D>( "DamageCollider" );
		_rotationPoint	 = GetNode<Node2D>			( "RotationPoint" );
		_directionSprite = GetNode<Sprite2D>		( "RotationPoint/DirectionSprite" );
		_playerSprite	 = GetNode<Sprite2D>		( "CharacterSprite" );
		
		GetNode<Sprite2D>( "CharacterSprite" ).Texture = _playerTexture;
		
		Rect2 rect = _collisionShape.Shape.GetRect();
		_rectPoints = new Vector2[]
		{
			// Get points on box
			rect.Position,                                                  // Top-Left
			new Vector2( rect.Position.X + rect.Size.X, rect.Position.Y ),  // Top-Right
			new Vector2( rect.Position.X,               -rect.Position.Y ), // Bottom-Left
			new Vector2( rect.Position.X + rect.Size.X, -rect.Position.Y )  // Bottom-Right
		};
	}
	
	public override void _Input( InputEvent @event )
	{
		// Mouse & keyboard
		if ( @event is InputEventMouseButton { Pressed: true } && _gameManager.UseMouse )
			InstantiateGrenade( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) );
		// Keyboard only
		if ( Input.IsActionPressed( _controls.Attack ) )
		{
			switch ( _currentWeapon )
			{
				case EWeapon.Bazooka:
					InstantiateBazooka( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPowerBazooka );
					break;
				case EWeapon.Grenade:
					InstantiateGrenade( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPowerGrenade );
					break;
				case EWeapon.Molotov:
					//InstantiateMolotov( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPower );
					break;
				default:
					break;
			}
		}
		if ( Input.IsActionPressed( _controls.Switch ) )
		{
			switch ( _currentWeapon )
			{
				case EWeapon.Bazooka:
					_currentWeapon = EWeapon.Grenade;
					break;
				case EWeapon.Grenade:
					_currentWeapon = EWeapon.Molotov;
					break;
				case EWeapon.Molotov:
					_currentWeapon = EWeapon.Bazooka;
					break;
				default:
					break;
			}
		}
		if ( Input.IsActionPressed( _controls.AimLeft ) )
			_rotationPoint.RotationDegrees -= 22.5f;
		if ( Input.IsActionPressed( _controls.AimRight ) )
			_rotationPoint.RotationDegrees += 22.5f;
	}

	public void InstantiateGrenade( Vector2 dir )
	{
		PackedScene scene = GD.Load<PackedScene>( "res://grenade_big.tscn" );
		GrenadeBig instance = ( GrenadeBig )scene.Instantiate();
		instance.Position = Position + new Vector2( 0, -5.0f ); // Position it 10px above
		instance.Init( dir );
		GetParent().AddChild( instance );
	}
	public void InstantiateBazooka( Vector2 dir )
	{
		PackedScene scene = GD.Load<PackedScene>( "res://bazooka.tscn" );
		Bazooka instance = ( Bazooka )scene.Instantiate();
		instance.Position = Position + new Vector2( 0, -5.0f ); // Position it 10px above
		instance.Init( dir );
		GetParent().AddChild( instance );
	}
	// public void InstantiateMolotov( Vector2 dir )
	// {
	// 	PackedScene scene = GD.Load<PackedScene>( "res://bazooka.tscn" );
	// 	Molotov instance = ( Molotov )scene.Instantiate();
	// 	instance.Position = Position + new Vector2( 0, -5.0f ); // Position it 10px above
	// 	instance.Init( dir );
	// 	GetParent().AddChild( instance );
	// }

	public override void _Process( double delta )
	{
		if ( Health <= 0 )
		{
			QueueFree();
		}
	}
	public override void _PhysicsProcess( double delta )
	{
		Vector2 validPos;
		int walk = 0;

		InternalVelocity.Y -= -12.82f * (float)delta;
		InternalVelocity.X -= InternalVelocity.X;
		
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			IsAirborne = false;
			InternalVelocity = Vector2.Zero;
			if ( Input.IsActionPressed( _controls.Jump ) )
			{
				IsAirborne         =  true;
				InternalVelocity.Y -= 7.0f;
			}
		}
		else
			IsAirborne = true;

		if ( IsAirborne )
		{
			// Apply Velocity
			Position += InternalVelocity;
		}
		
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0.0f, -1.0f ) ) == Vector2.Down )
		{
			// Jump through overhangs fix
			InternalVelocity.Y = 0.0f;
		}
		
		walk = Convert.ToInt32( Input.GetActionStrength( _controls.MoveRight ) ) -
			   Convert.ToInt32( Input.GetActionStrength( _controls.MoveLeft ) );

		// Flip sprite based on direction
		if ( walk < 0 )
		{
			if ( !_isSpriteFlipped )
				_playerSprite.FlipH = true;
			_isSpriteFlipped = true;
		}
		if ( walk > 0 )
		{
			if ( _isSpriteFlipped )
				_playerSprite.FlipH = false;
			_isSpriteFlipped = false;
		}
		
		validPos = Position; // current position is valid and our fallback

		// Where to walk, for loop is for slopes
		for ( int i = -3; i < 4; i++ )
		{
			Vector2 dir  = new Vector2( walk, i );
			Vector2 pos  = Position + dir;
				
			// new position doesn't have a normal -> it's valid to move to
			if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( pos ) == Vector2.Zero )
				validPos = pos;
			else
				break;
		}

		Position = validPos;
		Position += GetNode<Map>( "../Map" ).CollisionNormalPoint( validPos );
	}

	public override void _Draw()
	{
		//DrawLine( _rectPoints[ 0 ], _rectPoints[ 1 ], Colors.Aqua, 1.0f );
		//DrawLine( _rectPoints[ 1 ], _rectPoints[ 3 ], Colors.Beige, 1.0f );
		//DrawLine( _rectPoints[ 3 ], _rectPoints[ 2 ], Colors.Brown, 1.0f );
		//DrawLine( _rectPoints[ 2 ], _rectPoints[ 0 ], Colors.Red, 1.0f );
		
		//DrawCircle( Position, 1.0f, Colors.Aqua );
		//DrawCircle( _rectPoints[ 0 ], 1.0f, Colors.Red );
		//DrawCircle( _rectPoints[ 1 ], 1.0f, Colors.Aqua );
		//DrawCircle( _rectPoints[ 2 ], 1.0f, Colors.Aqua );
		//DrawCircle( _rectPoints[ 3 ], 1.0f, Colors.Aqua );
	}
}
