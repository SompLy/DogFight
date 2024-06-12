using Godot;
using System;

public partial class Drill : Node2D
{
	private Map _map;
	public override void _Ready()
	{
		_map = GetParent().GetParent().GetNode<Map>( "Map" );
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 normal;
		normal = _map.CollisionNormalPoint( GlobalPosition + new Vector2( 0.0f, 1.0f ) );
		
		if ( normal == Vector2.One )
		{
			// Drilling
			Node2D parent = ( Node2D )GetParent();
			parent.Position -= new Vector2( 0.0f, 200.0f * ( float )delta );
		}
		else
		{
			// Drilling done
			Controller controller = ( Controller )GetParent();
			controller.IsDrilling = false;
			QueueFree();
		}
	}
}
