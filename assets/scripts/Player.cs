using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private CollisionShape2D _collisionShape;
	private Vector2[]        _rectPoints;
	private Map				 _map;
	
	private int _jump    = 0;
	private int _jumpDir = 0;
	private Vector2 _dir;

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
		//_dir.Y += Gravity / Mathf.Max( _dir.Length(), 1 );
		DoSteps();
		
		Vector2 validPos;
		if ( _jump > 0 )
		{
			validPos = Position; // We are currently on a valid position
			
			// Possible pixel player can jump to 
			for ( int i = 0; i > -6; i-- )
			{
				Vector2 dir = new Vector2( _jumpDir, i );
				Vector2 pos = Position + dir;
				
				// new position doesn't have a normal -> it's valid to move to
				if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( pos ) == Vector2.Zero )
					validPos = pos;
			}

			_jump--;              // reduce the jump counter
			Position = validPos; // move to the next valid position
		}

		//int walk = _jumpDir; // set walk to jumpDir in case we are falling
		int walk = 0;
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			
			_jumpDir = 0; // Not jumping
			if ( Input.IsActionPressed( _controls.Jump ) )
			{
				// we are trying to jump
				if ( _jump == 0 ) // currently not traveling upwards
				{
					_jumpDir = Convert.ToInt32( Input.GetActionStrength( _controls.MoveRight ) ) -
							  Convert.ToInt32( Input.GetActionStrength( _controls.MoveLeft ) );
					_jump = 20; // Travel upwards for x frames
				}
			}

			walk = Convert.ToInt32( Input.GetActionStrength( _controls.MoveRight ) ) -
				   Convert.ToInt32( Input.GetActionStrength( _controls.MoveLeft ) );
		}

		validPos = Position; // current position is valid and our fallback

		for ( int i = -3; i < 4; i++ )
		{
			Vector2 dir  = new Vector2( walk, i );
			Vector2 pos  = Position + dir;
				
			// new position doesn't have a normal -> it's valid to move to
			if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( pos ) == Vector2.Zero )
				validPos = pos;
		}

		Position = validPos;
		
		Position += GetNode<Map>( "../Map" ).CollisionNormalPoint( validPos );
	}

	private void DoSteps()
	{
		var velocity = _dir;
		
		while ( Mathf.Abs( velocity.Y ) > 0 )
		{
			var newPosition = Position +
			                  ( Vector2.Down * Mathf.Sign( velocity.Y ) * Mathf.Min( Mathf.Abs( velocity.Y ), 1.0f ) );
			var normal = _map.CollisionNormalPoint( newPosition );
			velocity.Y -= Mathf.Min( 1.0f, Mathf.Abs( velocity.Y ) ) * Mathf.Sign( velocity.Y );
	
			if ( normal == Vector2.One )
				break;
	
			if ( Mathf.Sign( normal.Y ) != 0 && Mathf.Sign( _dir.Y ) != Mathf.Sign( normal.Y ) )
			{
				_dir.Y     *= -0.5f;
				velocity.Y *= -0.5f;
			}
	
			if ( Mathf.Sign( normal.X ) != 0 && Mathf.Sign( _dir.X ) != Mathf.Sign( normal.X ) )
			{
				_dir.X     *= -0.8f;
				velocity.X *= -0.8f;
			}
	
			Position = newPosition;
		}
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
