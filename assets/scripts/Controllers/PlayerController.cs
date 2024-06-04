using Godot;
using System;

public partial class PlayerController : Controller
{
	[Export] public  PlayerControls  Controls;
	
	public override void _Ready()
	{
		base._Ready();
	}
	public override void _Input( InputEvent @event )
	{
		// Mouse & keyboard
		if ( @event is InputEventMouseButton { Pressed: true } && _GameManager.UseMouse )
			InstantiateGrenade( GetGlobalMousePosition() - GlobalPosition + new Vector2( 0, -12 ) );
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
