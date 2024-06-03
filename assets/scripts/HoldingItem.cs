using Godot;
using System;

public partial class HoldingItem : Sprite2D
{
	private Node2D _rotationPoint;
	private PlayerController _playerController;
	private Sprite2D _spriteInstance;

	public override void _Ready()
	{
		_rotationPoint = GetNode<Node2D>( "../RotationPoint" );
		_playerController        = ( PlayerController )GetParent();
	}

	public override void _Process( double delta )
	{
		if ( _playerController.ShouldSwitchWeapon )
		{
			_playerController.ShouldSwitchWeapon = false;
			
			switch ( _playerController.CurrentWeapon )
			{
				case PlayerController.EWeapon.Bazooka:
					_spriteInstance =
						( Sprite2D )GD.Load<PackedScene>( "res://assets/sprites/bazooka_sprite.tscn" ).Instantiate();
					break;
				case PlayerController.EWeapon.Grenade:
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
