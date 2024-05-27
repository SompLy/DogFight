using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private CollisionShape2D _collisionShape;
	private Vector2[]        _rectPoints;
	private Map				 _map;

	private bool _isAirborne;
	private Vector2 _velocity;

	private const float Gravity = 0.1f;

	[Export] private PlayerControls _controls;
	[Export] private Texture2D _playerTexture;
	
	public override void _Ready()
	{
		_map = GetNode<Map>( "../Map" );
		_collisionShape = GetNode<CollisionShape2D>( "Collider" );
		GetNode<Sprite2D>( "Sprite2D" ).Texture = _playerTexture;
		
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
		if ( @event is InputEventMouseButton mouseButton && mouseButton.Pressed )
		{
			PackedScene scene = GD.Load<PackedScene>( "res://grenade_big.tscn" );
			GrenadeBig instance = ( GrenadeBig )scene.Instantiate();
			instance.Position = Position + new Vector2( 0, -12 ); // Position it 12px above our origin
			// `grenade.Init()` takes as argument the direction that the grenade will fly in
			// we subtract the grenade's global position from the mouse global position to get that
			instance.Init( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) );
			GetParent().AddChild( instance ); // Add the grenade as a child of our parent (`Main`)
		}
	}

	public override void _PhysicsProcess( double delta )
	{
		Vector2 validPos;
		int walk = 0;

		if ( _isAirborne )
		{
			ApplyVelocity();
		}
		
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			_isAirborne = false;
			if ( Input.IsActionPressed( _controls.Jump ) )
			{
				_isAirborne = true;
				_velocity.Y -= 10.0f;
			}
		}

		//if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0.0f, -1.0f )) != Vector2.Down )
		//{
			// Jump through overhangs fix
			//_velocity.Y = 1.0f;
		//}
		
		walk = Convert.ToInt32( Input.GetActionStrength( _controls.MoveRight ) ) -
			   Convert.ToInt32( Input.GetActionStrength( _controls.MoveLeft ) );

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
		Position += _velocity;
		Position += GetNode<Map>( "../Map" ).CollisionNormalPoint( validPos );
	}

	private void ApplyVelocity()
	{
		// Decerase velocity
		_velocity.Y -= Mathf.Min( 1.0f, Mathf.Abs( _velocity.Y ) ) * Mathf.Sign( _velocity.Y ) * 0.5f;
		_velocity.X -= Mathf.Min( 1.0f, Mathf.Abs( _velocity.X ) ) * Mathf.Sign( _velocity.X );
		
		// Calculate new position
		var newPosition = Position +
		                  ( Vector2.Down * 0.1f * Mathf.Sign( _velocity.Y ) * Mathf.Min( Mathf.Abs( _velocity.Y ), 1.0f ) );
		
		Position = newPosition;
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
