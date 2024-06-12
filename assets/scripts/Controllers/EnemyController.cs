using Godot;
using System;

public partial class EnemyController : Controller
{
	public Vector2			DistanceToPlayer = Vector2.Zero;
	public PlayerController PlayerController;
	public override void _Ready()
	{
		base._Ready();

		PlayerController = GetNode<PlayerController>( "../Player1" );
	}
	public override void _Process(double delta)
	{
		base._Process( delta );
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess( delta );

		DistanceToPlayer = Position - PlayerController.Position;
	}
}
