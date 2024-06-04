using Godot;
using System;

public partial class Bazooka : Projectile
{
	private Map _map;
	private GpuParticles2D _particleInstance;
	public override void _Ready()
	{
		base._Ready();
		_map = GetNode<Map>( "../Map" );
	}

	public override void _PhysicsProcess( double delta )
	{
		base._PhysicsProcess( delta );

		if ( Hit && !IsInstanceValid( _particleInstance ))
		{
			_map.Explosion( Position, 40 );
			GetNode<Sprite2D>( "Sprite2D" ).Visible = false;
			GetNode<GpuParticles2D>( "BazookaParticle" ).Visible = false;
			Dir = Vector2.Zero;
			InstantiateParticle();
		}
	}
	
	public void InstantiateParticle( )
	{
		PackedScene scene = GD.Load<PackedScene>( "res://assets/ParticleEffects/particle_explosion.tscn" );
		_particleInstance = ( GpuParticles2D )scene.Instantiate();
		AddChild( _particleInstance );
		_particleInstance.Emitting = true;
	}
}
