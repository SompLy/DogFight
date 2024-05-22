using Godot;
using System;

public partial class GrenadeBig : Node2D
{
	private Label _label;
	private Timer _timer;
	private Map   _map;
	public override void _Ready()
	{
		_label = GetNode<Label>( "Label" );
		_timer = GetNode<Timer>( "Timer" );
		_map = GetNode<Map>( "../Map" );
	}
	private const float GRAVITY = 0.1f;
	private Vector2 _dir;
	
	public void Init( Vector2 dir )
	{
		_dir = dir.Normalized() * Mathf.Max( 2, dir.Length() * 0.01f );
	}
	
	public override void _Process( double delta )
	{
		_label.Text = _timer.TimeLeft.ToString();
	}

	public override void _PhysicsProcess( double delta )
	{
		_label.Text = Mathf.Ceil( _timer.TimeLeft ).ToString();
		
		 if ( _timer.TimeLeft <= 0 )
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
	
	 	while ( Mathf.Abs( velocity.X ) > 0 )
	 	{
	 		var newPosition = Position +
	 						  ( Vector2.Right * Mathf.Sign( velocity.X ) * Mathf.Min( Mathf.Abs( velocity.X ), 1.0f ) );
	 		var normal = _map.CollisionNormalPoint( newPosition );
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

		    Rotation += velocity.X * 0.1f;
	    }
	 }

	 public void _OnTimerTimeout()
	 {
		 _map.Explosion( Position, 30 );
	    _label.Visible = false;
	 	GetNode<Sprite2D>( "Sprite2D" ).Visible = false;
	 	//GetNode<AnimationPlayer>("Explosion").Visable = true;
	 	//GetNode<AnimationPlayer>("Explosion").Play("default");
	 	QueueFree();
	 }
	
	 public void _OnExplosionAnimationFinished()
	 {
	 }
}
