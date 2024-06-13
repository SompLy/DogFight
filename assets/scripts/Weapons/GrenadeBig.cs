using Godot;
using System;

public partial class GrenadeBig : Throwable
{
	private Label _label;
	private Timer _timer;
	private Map _map;
	private GpuParticles2D _particleInstance;
	
	public override void _Ready()
	{
		base._Ready();
		_label = GetNode<Label>( "Label" );
		_timer = GetNode<Timer>( "Timer" );
		_map   = GetNode<Map>( "../Map" );
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

		base._PhysicsProcess( delta );
	}

	public void _OnTimerTimeout()
	{
		_map.Explosion( Position, 30, OwnerIndex );
		_label.Visible                          = false;
		GetNode<Sprite2D>( "Sprite2D" ).Visible = false;
		InstantiateParticle();
	}
	public void InstantiateParticle( )
	{
		PackedScene scene = GD.Load<PackedScene>( "res://assets/ParticleEffects/particle_explosion.tscn" );
		_particleInstance = ( GpuParticles2D )scene.Instantiate();
		AddChild( _particleInstance );
		_particleInstance.Emitting = true;
	}
}
