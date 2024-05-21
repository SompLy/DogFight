using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private CollisionShape2D _collisionShape;
	private Vector2[]        _rectPoints;
	
	private int _jump    = 0;
	private int _jumpDir = 0;

	// Get the gravity from the project settings
	private float _gravity = ProjectSettings.GetSetting( "physics/2d/default_gravity" ).AsSingle();

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>( "Collider" );
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
			Grenade grenade = new Grenade();
			grenade.Position = Position + new Vector2( 0, -12 ); // Position it 12px above our origin
			// `grenade.Init()` takes as argument the direction that the grenade will fly in
			// we subtract the grenade's global position from the mouse global position to get that
			grenade.Init( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) );
			GetParent().AddChild( grenade ); // Add the grenade as a child of our parent (`Main`)
		}
	}

	public override void _PhysicsProcess( double delta )
	{
		//GD.Print( "X: " + Position.X.ToString() +
		//		  ", Y: " + Position.Y.ToString() + "\n" );
		
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
			return;              // No other controls allowed while in the air
		}

		int walk = _jumpDir; // set walk to jumpDir in case we are falling
		if ( GetNode<Map>( "../Map" ).CollisionNormalPoint( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// the pixel below us is solid.
			
			_jumpDir = 0; // Not jumping
			if ( Input.IsActionPressed( "Up" ) )
			{
				// we are trying to jump
				if ( _jump == 0 ) // currently not traveling upwards
				{
					_jumpDir = Convert.ToInt32( Input.GetActionStrength( "Right" ) ) -
							  Convert.ToInt32( Input.GetActionStrength( "Left" ) );
					_jump = 10; // Travel upwards for 10 frames
				}
			}

			walk = Convert.ToInt32( Input.GetActionStrength( "Right" ) ) -
				   Convert.ToInt32( Input.GetActionStrength( "Left" ) );
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
	public override void _Draw()
	{
		//DrawLine( _rectPoints[ 0 ], _rectPoints[ 1 ], Colors.Aqua, 1.0f );
		//DrawLine( _rectPoints[ 1 ], _rectPoints[ 3 ], Colors.Beige, 1.0f );
		//DrawLine( _rectPoints[ 3 ], _rectPoints[ 2 ], Colors.Brown, 1.0f );
		//DrawLine( _rectPoints[ 2 ], _rectPoints[ 0 ], Colors.Red, 1.0f );
		
		//DrawCircle( Position, 1.0f, Colors.Aqua );
		//DrawCircle( _rectPoints[ 0 ], 1.0f, Colors.Red );
		//DrawCircle( _rectPoints[ 1 ], 1.0f, Colors.Aqua );
		DrawCircle( _rectPoints[ 2 ], 1.0f, Colors.Aqua );
		DrawCircle( _rectPoints[ 3 ], 1.0f, Colors.Aqua );
	}
}
