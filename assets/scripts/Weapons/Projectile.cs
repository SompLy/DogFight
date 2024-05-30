using Godot;
using System;

public partial class Projectile : Node2D
{
	private Map _map;

	private const float GRAVITY = 0.1f;
	public Vector2 Dir;

	public bool Hit = false;
	
	public override void _Ready()
	{
		_map = GetNode<Map>( "../Map" );
	}

	public void Init( Vector2 dir )
	{
		Dir = dir.Normalized() * Mathf.Max( 2, dir.Length() * 0.01f );
	}

	public override void _PhysicsProcess( double delta )
	{
		Dir.Y += GRAVITY / Mathf.Max( Dir.Length(), 1 );
		ApplyVelocityWithCollisions ();
	}

	private void ApplyVelocityWithCollisions ()
	{
		Vector2 velocity = Dir;

		while ( Mathf.Abs( velocity.Y ) > 0 )
		{
			var newPosition = Position +
			                  Vector2.Down * Mathf.Sign( velocity.Y ) * Mathf.Min( Mathf.Abs( velocity.Y ), 1.0f );
			var normal = _map.CollisionNormalPoint( newPosition );
			velocity.Y -= Mathf.Min( 1.0f, Mathf.Abs( velocity.Y ) ) * Mathf.Sign( velocity.Y );

			if ( normal == Vector2.One )
				break;
			
			if ( normal != Vector2.Zero )
				Hit = true;
			
			LookAt( newPosition );
			Position = newPosition;
		}

		while ( Mathf.Abs( velocity.X ) > 0 )
		{
			var newPosition = Position +
			                  Vector2.Right * Mathf.Sign( velocity.X ) * Mathf.Min( Mathf.Abs( velocity.X ), 1.0f );
			var normal = _map.CollisionNormalPoint( newPosition );
			velocity.X -= Mathf.Min( 1.0f, Mathf.Abs( velocity.X ) ) * Mathf.Sign( velocity.X );

			if ( normal == Vector2.One )
				break;

			Position = newPosition;
			LookAt( newPosition );
		}
		
	}
}
