using Godot;
using System;

public partial class Controller : CharacterBody2D
{
	public GameManager _gameManager;
	
	private CollisionShape2D _collisionShape;
	private Vector2[]        _rectPoints;
	private Map				 _map;
	
	[Export] public  int		Health				= 3;
	[Export] public float		_attackPowerBazooka	= 40.0f;
	[Export] public float		_attackPowerGrenade	= 12.0f;
			 public  Vector2	InternalVelocity 	= Vector2.Zero;
			 public  bool		IsAirborne		 	= true;
			 private bool		_isSpriteFlipped 	= false;
			 public	 bool		ShouldSwitchWeapon	= true;
			 public int			Walk				= 0;		 
	 public enum EWeapon
	 {
		 Bazooka,
		 Grenade,
		 Molotov
	 }

	public EWeapon CurrentWeapon = EWeapon.Bazooka;
			 
	private const float Gravity = 0.1f;

	[Export] public  PlayerControls  Controls;
	[Export] private Texture2D		_playerTexture;
			 private Sprite2D		_playerSprite;
			 public  Node2D 		RotationPoint;
			 public Node2D 			DirectionSprite;
	
	public override void _Ready()
	{
		_gameManager	 = GetNode<GameManager>		( "/root/GameManager" );
		_map			 = GetNode<Map>				( "../Map" );
		_collisionShape  = GetNode<CollisionShape2D>( "DamageCollider" );
		RotationPoint	 = GetNode<Node2D>			( "RotationPoint" );
		DirectionSprite = GetNode<Sprite2D>		( "RotationPoint/DirectionSprite" );
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
	
	public override void _Process( double delta )
	{
		if ( Health > 0 ) return;
		
		Death();
		QueueFree();
	}
	public override void _PhysicsProcess( double delta )
	{
		Vector2 validPos;

		InternalVelocity.Y -= -12.82f * (float)delta;
		InternalVelocity.X -= InternalVelocity.X;
		
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			IsAirborne       = false;
			InternalVelocity = Vector2.Zero;
			if ( Input.IsActionPressed( Controls.Jump ) )
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

		// Flip sprite based on direction
		if ( Walk < 0 )
		{
			if ( !_isSpriteFlipped )
				_playerSprite.FlipH = true;
			_isSpriteFlipped = true;
		}
		if ( Walk > 0 )
		{
			if ( _isSpriteFlipped )
				_playerSprite.FlipH = false;
			_isSpriteFlipped = false;
		}
		
		validPos = Position; // current position is valid and our fallback

		// Where to walk, for loop is for slopes
		for ( int i = -3; i < 4; i++ )
		{
			Vector2 dir  = new Vector2( Walk, i );
			Vector2 pos  = Position + dir;
				
			// new position doesn't have a normal -> it's valid to move to
			if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( pos ) == Vector2.Zero )
				validPos = pos;
			else
				break;
		}

		Position =  validPos;
		Position += GetNode<Map>( "../Map" ).CollisionNormalPoint( validPos );
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
	private void Death()
	{
		_gameManager.PlayerDeath( Controls.PlayerIndex );
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
