using Godot;
using System;

public partial class Grenade : Node2D
{
	private const float GRAVITY = 0.1f;

	private Vector2 _dir;

	public void Init( Vector2 dir )
	{
		_dir = dir.Normalized() * Mathf.Max( 2, dir.Length() * 0.01f );
	}

	public override void _Ready()
	{
		GD.Print( "epic" );
		GD.Print( GetNode<Sprite2D>( "../Timer" ).Name );
	}

	public override void _PhysicsProcess( double delta )
	{
		GD.Print( GetNode<Timer>( "Timer" ).TimeLeft );
		GetNode<Label>( "Label" ).Text = Mathf.Ceil( GetNode<Timer>( "Timer" ).TimeLeft ).ToString();

		if ( GetNode<Timer>( "Timer" ).TimeLeft <= 0 )
			return;

		_dir.Y += GRAVITY / Mathf.Max( _dir.Length(), 1 );
		DoSteps();
	}

	private void DoSteps()
	{
		var velocity = _dir;

		while ( Mathf.Abs( velocity.Y ) > 0 )
		{
			var newPosition = Position +
							  ( Vector2.Down * Mathf.Sign( velocity.Y ) * Mathf.Min( Mathf.Abs( velocity.Y ), 1.0f ) );
			var normal = GetNode<Map>( "../Map" ).CollisionNormalPoint( newPosition );
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

		while ( Mathf.Abs( velocity.X ) > 0 )
		{
			var newPosition = Position +
							  ( Vector2.Right * Mathf.Sign( velocity.X ) * Mathf.Min( Mathf.Abs( velocity.X ), 1.0f ) );
			var normal = GetNode<Map>( "../Map" ).CollisionNormalPoint( newPosition );
			velocity.X -= Mathf.Min( 1.0f, Mathf.Abs( velocity.X ) ) * Mathf.Sign( velocity.X );

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

	public void _OnTimerTimeout()
	{
		GetNode<Map>( "../Map" ).Explosion( Position, 30 );
		GetNode<Label>( "Label" ).Visible       = false;
		GetNode<Sprite2D>( "Sprite2D" ).Visible = false;
		//GetNode<AnimationPlayer>("Explosion").Visable = true;
		//GetNode<AnimationPlayer>("Explosion").Play("default");
	}

	public void _OnExplosionAnimationFinished()
	{
		QueueFree();
	}
}
