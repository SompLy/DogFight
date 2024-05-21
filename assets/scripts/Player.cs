using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public int Jump    = 0;
	public int JumpDir = 0;

	// Get the gravity from the project settings
	public float gravity = ProjectSettings.GetSetting( "physics/2d/default_gravity" ).AsSingle();

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
		GD.Print( "X: "                 +
				  Position.X.ToString() +
				  ", Y: "               +
				  Position.Y.ToString() +
				  "\n" );
		Vector2 validPos;
		if ( Jump > 0 )
		{
			validPos = Position; // We are currently on a valid position

			for ( int i = 0; i > -6; i-- )
			{
				Vector2 dir = new Vector2( JumpDir, i );
				var     pos = Position + dir;
				if ( GetNode<Map>( "../Map" ).CollisionNormal( pos ) == Vector2.Zero )
				{
					// new position doesn't have a normal -> it's valid to move to
					validPos = pos;
				}
			}

			Jump--;              // reduce the jump counter
			Position = validPos; // move to the next valid position
			return;              // No other controls allowed while in the air
		}

		var walk = JumpDir; // set walk to jumpDir in case we are falling
		if ( GetNode<Map>( "../Map" ).CollisionNormal( Position + new Vector2( 0, 1 ) ) != Vector2.Zero )
		{
			// GD.Print("X: " +
			// 	GetNode<Map>("../Map").CollisionNormal(Position ).X.ToString() +
			// 		", Y: " +
			// 		 GetNode<Map>("../Map").CollisionNormal(Position ).Y.ToString() +
			// 	"\n");
			// the pixel below us is solid.
			JumpDir = 0; // reset jumpDir (we're not jumping)
			if ( Input.IsActionJustPressed( "Up" ) )
			{
				// we are trying to jump
				if ( Jump == 0 ) // currently not rising
				{
					JumpDir = Convert.ToInt32( Input.GetActionStrength( "Right" ) ) -
							  Convert.ToInt32( Input.GetActionStrength( "Left" ) );
					Jump = 10; // We'll be rising for 10 frames
				}
			}

			walk = Convert.ToInt32( Input.GetActionStrength( "Right" ) ) -
				   Convert.ToInt32( Input.GetActionStrength( "Left" ) );
		}

		validPos = Position; // current position is valid and our fallback

		for ( int i = -3; i < 4; i++ )
		{
			Vector2 dir = new Vector2( walk, i );
			var     pos = Position + dir;
			if ( GetNode<Map>( "../Map" ).CollisionNormal( pos ) == Vector2.Zero )
			{
				validPos = pos;
			}
		}

		Position = validPos;

		Position += GetNode<Map>( "../Map" ).CollisionNormal( Position );
	}
}
