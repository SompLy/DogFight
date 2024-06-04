using Godot;
using System;

public partial class Controller : CharacterBody2D
{
	public GameManager _GameManager;
	public MultiTargetDynamicCamera2D _MultiTargetDynamicCamera2D;

	private CollisionShape2D _collisionShape;
	private Vector2[] _rectPoints;
	private Map _map;

	[Export] public bool AddToCamera = false;
	[Export] public int PlayerIndex = 0;
	[Export] public int Health = 3;
	public float AttackPowerBazooka = 40.0f;
	public float AttackPowerGrenade = 12.0f;
	public Vector2 InternalVelocity = Vector2.Zero;
	public bool IsAirborne = true;
	public bool ShouldSwitchWeapon = true;
	public int Walk = 0;
	public bool ShouldJump = false;

	public enum EWeapon
	{
		Bazooka,
		Grenade,
		Molotov
	}

	public EWeapon CurrentWeapon = EWeapon.Bazooka;

	private const float Gravity = 0.1f;

	[Export] public Texture2D PlayerTexture;
	private Sprite2D _playerSprite;
	public Node2D RotationPoint;
	public Node2D DirectionSprite;

	public override void _Ready()
	{
		_MultiTargetDynamicCamera2D = GetNode<MultiTargetDynamicCamera2D>( "../MultiTargetDynamicCamera2D" );

		_GameManager    = GetNode<GameManager>( "/root/GameManager" );
		_map            = GetNode<Map>( "../Map" );
		_collisionShape = GetNode<CollisionShape2D>( "DamageCollider" );
		RotationPoint   = GetNode<Node2D>( "RotationPoint" );
		DirectionSprite = GetNode<Sprite2D>( "RotationPoint/DirectionSprite" );
		_playerSprite   = GetNode<Sprite2D>( "CharacterSprite" );

		if ( AddToCamera )
		{
			GD.Print( "Added \"" + Name + "\" to dynamic camera" );
			_MultiTargetDynamicCamera2D.AddTarget( Position );
		}

		GD.Print( "Total targets : " + _MultiTargetDynamicCamera2D.GetTargets().Count );

		GetNode<Sprite2D>( "CharacterSprite" ).Texture = PlayerTexture;

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
		_MultiTargetDynamicCamera2D.UpdateTarget( Position, PlayerIndex );

		if ( Health > 0 ) return;
		Death();
		QueueFree();
	}

	public override void _PhysicsProcess( double delta )
	{
		Vector2 validPos;

		InternalVelocity.Y -= -12.82f * ( float )delta;
		InternalVelocity.X -= InternalVelocity.X;

		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			IsAirborne       = false;
			InternalVelocity = Vector2.Zero;
			if ( ShouldJump )
			{
				ShouldJump         =  false;
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
			if ( !_playerSprite.FlipH )
				_playerSprite.FlipH = true;
		}

		if ( Walk > 0 )
		{
			if ( _playerSprite.FlipH )
				_playerSprite.FlipH = false;
		}

		validPos = Position; // current position is valid and our fallback

		// Where to walk, for loop is for slopes
		for ( int i = -3; i < 4; i++ )
		{
			Vector2 dir = new Vector2( Walk, i );
			Vector2 pos = Position + dir;

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
		_GameManager.PlayerDeath( PlayerIndex );
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
