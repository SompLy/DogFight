using Godot;
using System;

public partial class PlayerController : Controller
{
	[Export] public  PlayerControls  Controls;
	
	public Node2D RotationPoint;
	public Node2D DirectionSprite;
	public override void _Ready()
	{
		base._Ready();
			
		RotationPoint   = GetNode<Node2D>( "RotationPoint" );
		DirectionSprite = GetNode<Sprite2D>( "RotationPoint/DirectionSprite" );
		if ( _GameManager.GameMode != GameManager.EGameMode.PvP )
			DirectionSprite.Visible = false;
	}
	public override void _Input( InputEvent @event )
	{
		// Mouse & keyboard
		if ( @event is InputEventMouseButton { Pressed: true } && _GameManager.UseMouse )
		{
			switch ( CurrentWeapon )
			{
				case EWeapon.Bazooka:
					InstantiateBazooka( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) 
						* AttackPowerBazooka * 0.5f );
					break;
				case EWeapon.Grenade:
					InstantiateGrenade( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) );
					break;
				case EWeapon.Molotov:
					//InstantiateMolotov( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPower );
					break;
				default:
					break;
			}
		}
		// Keyboard only
		if ( Input.IsActionPressed( Controls.Attack ) )
		{
			switch ( CurrentWeapon )
			{
				case EWeapon.Bazooka:
					InstantiateBazooka( ( DirectionSprite.GlobalPosition - RotationPoint.GlobalPosition ) * AttackPowerBazooka );
					break;
				case EWeapon.Grenade:
					InstantiateGrenade( ( DirectionSprite.GlobalPosition - RotationPoint.GlobalPosition ) * AttackPowerGrenade );
					break;
				case EWeapon.Molotov:
					//InstantiateMolotov( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPower );
					break;
				default:
					break;
			}
		}
		if ( Input.IsActionPressed( Controls.Switch ) )
		{
			ShouldSwitchWeapon = true;
			
			switch ( CurrentWeapon )
			{
				case EWeapon.Bazooka:
					CurrentWeapon = EWeapon.Grenade;
					break;
				case EWeapon.Grenade:
					CurrentWeapon = EWeapon.Bazooka;
					break;
				// case EWeapon.Molotov:
				// 	CurrentWeapon = EWeapon.Bazooka;
				// 	break;
				default:
					break;
			}
		}
		if ( Input.IsActionPressed( Controls.AimLeft ) )
			RotationPoint.RotationDegrees -= 22.5f;
		if ( Input.IsActionPressed( Controls.AimRight ) )
			RotationPoint.RotationDegrees += 22.5f;
	}
	public override void _Process(double delta)
	{
		base._Process( delta );

	}
	public override void _PhysicsProcess(double delta)
	{
		if ( Input.IsActionJustPressed( Controls.Jump ) && !ShouldJump )
		{
			ShouldJump = true;
		}

		Walk = Convert.ToInt32( Input.GetActionStrength( Controls.MoveRight ) ) -
		       Convert.ToInt32( Input.GetActionStrength( Controls.MoveLeft ) );
		
		base._PhysicsProcess( delta );
	}
}
