using Godot;
using System;

public partial class Bazooka : Projectile
{
	private Map _map;

	public override void _Ready()
	{
		base._Ready();
		_map   = GetNode<Map>( "../Map" );
	}

	public override void _PhysicsProcess( double delta )
	{
		base._PhysicsProcess( delta );
	}

	public void _OnTimerTimeout()
	{
		_map.Explosion( Position, 20 );
		GetNode<Sprite2D>( "Sprite2D" ).Visible = false;
		//GetNode<AnimationPlayer>("Explosion").Visable = true;
		//GetNode<AnimationPlayer>("Explosion").Play("default");
		QueueFree();
	}

	public void _OnExplosionAnimationFinished()
	{
	}
}
