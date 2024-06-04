using Godot;
using System;

public partial class HoldingItem : Sprite2D
{
	private Node2D	   _rotationPoint;
	private Controller _controller;
	private Sprite2D   _spriteInstance;

	public override void _Ready()
	{
		_rotationPoint = GetNode<Node2D>( "../RotationPoint" );
		_controller	   = ( Controller )GetParent();
	}

	public override void _Process( double delta )
	{
		if ( _controller.ShouldSwitchWeapon )
		{
			_controller.ShouldSwitchWeapon = false;
			
			switch ( _controller.CurrentWeapon )
			{
				case Controller.EWeapon.Bazooka:
					_spriteInstance =
						( Sprite2D )GD.Load<PackedScene>( "res://assets/sprites/bazooka_sprite.tscn" ).Instantiate();
					break;
				case Controller.EWeapon.Grenade:
					_spriteInstance =
						( Sprite2D )GD.Load<PackedScene>( "res://assets/sprites/grenade_sprite.tscn" ).Instantiate();
					break;
				// case Player.EWeapon.Molotov:
				// 	InstantiateMolotov( ( _directionSprite.GlobalPosition - _rotationPoint.GlobalPosition ) * _attackPower );
				// 	break;
				default:
					break;
			}
		}

		Texture  = _spriteInstance.Texture;
		Scale    = _spriteInstance.Scale;
		Rotation = _rotationPoint.Rotation + Mathf.Pi * 0.5f;
	}
}
