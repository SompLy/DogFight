using Godot;
using System;

public partial class Throwable : Node2D
{
	private Map _map;
	private const float GRAVITY = 0.1f;
	private Vector2 _dir;

	public override void _Ready()
	{
		_map = GetNode<Map>( "../Map" );
	}

	public void Init( Vector2 dir )
	{
		_dir = dir.Normalized() * Mathf.Max( 2, dir.Length() * 0.01f );
	}

	public override void _PhysicsProcess( double delta )
	{
		_dir.Y += GRAVITY / Mathf.Max( _dir.Length(), 1 );
		ApplyVelocityWithCollisions ();
	}

	private void ApplyVelocityWithCollisions ()
	{
		var velocity = _dir;

		while ( Mathf.Abs( velocity.Y ) > 0 )
		{
			var newPosition = Position +
			                  Vector2.Down * Mathf.Sign( velocity.Y ) * Mathf.Min( Mathf.Abs( velocity.Y ), 1.0f );
			var normal = _map.CollisionNormalPoint( newPosition );
			velocity.Y -= Mathf.Min( 1.0f, Mathf.Abs( velocity.Y ) ) * Mathf.Sign( velocity.Y );

			if ( normal == Vector2.One )
			{
				break;
			}

			if ( normal.Y != 0 && Mathf.Sign( _dir.Y ) != Mathf.Sign( normal.Y ) )
			{
				_dir.Y     *= -0.5f;
				velocity.Y *= -0.5f;
			}

			Position = newPosition;
		}

		while ( Mathf.Abs( velocity.X ) > 0 )
		{
			var newPosition = Position +
			                  ( Vector2.Right * Mathf.Sign( velocity.X ) * Mathf.Min( Mathf.Abs( velocity.X ), 1.0f ) );
			var normal = _map.CollisionNormalPoint( newPosition );
			velocity.X -= Mathf.Min( 1.0f, Mathf.Abs( velocity.X ) ) * Mathf.Sign( velocity.X );

			if ( normal == Vector2.One )
			{
				break;
			}

			if ( normal.X != 0 && Mathf.Sign( _dir.X ) != Mathf.Sign( normal.X ) )
			{
				_dir.X     *= -0.1f;
				velocity.X *= -0.1f;
			}

			Position = newPosition;

			Rotation += velocity.X * 0.1f;
		}
	}
}
