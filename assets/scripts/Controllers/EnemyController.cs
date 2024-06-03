using Godot;
using System;

public partial class EnemyController : Controller
{
	enum EState
	{
		Idle,
		Walking,
		BazookaShort,
		BazookaLong,
		Grenade
	}

	private EState _state = EState.Idle;
	
	public override void _Ready()
	{
		base._Ready();
		
	}
	public override void _Process(double delta)
	{
		base._Process( delta );

		switch ( _state )
		{
			case EState.Idle:
				
				break;
			case EState.Walking:
				break;
			case EState.BazookaShort:
				break;
			case EState.BazookaLong:
				break;
			case EState.Grenade:
				break;
			default:
				break;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess( delta );
		
	}
}
