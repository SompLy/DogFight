using Godot;
using System;

public partial class InstantiateControllers : Node
{
	private PackedScene _player;
	private PackedScene _enemy;
	private PackedScene _map;
	private PackedScene _camera;
	private GameManager _gameManager;
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>( "/root/GameManager" );
		_player      = ResourceLoader.Load<PackedScene>( "res://player_controller.tscn" );
		_enemy       = ResourceLoader.Load<PackedScene>( "res://enemy_controller.tscn" );
		_map         = ResourceLoader.Load<PackedScene>( "res://map.tscn" );
		_camera		 = ResourceLoader.Load<PackedScene>( "res://multi_target_dynamic_camera_2d.tscn" );
		
		// Player 1
		PlayerController p1Instance = ( PlayerController )_player.Instantiate();
		p1Instance.Name          = "Player1";
		p1Instance.Controls      = ( PlayerControls )GD.Load( "res://assets/Resource/p1_Controls.tres" );
		p1Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Collie.png" );
		p1Instance.AddToCamera   = true;
		p1Instance.PlayerIndex   = 0;
		p1Instance.Health        = 3;
		p1Instance.Position      = new Vector2( 100.0f, 100.0f );
		
		// Player 2
		PlayerController p2Instance = ( PlayerController )_player.Instantiate();
		p2Instance.Name          = "Player2";
		p2Instance.Controls      = ( PlayerControls )GD.Load( "res://assets/Resource/p2_Controls.tres" );
		p2Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
		p2Instance.AddToCamera   = true;
		p2Instance.PlayerIndex   = 1;
		p2Instance.Health        = 3;
		p2Instance.Position      = new Vector2( 900.0f, 100.0f );

		EnemyController enemyInstance = ( EnemyController )_enemy.Instantiate();
		switch ( _gameManager.CurrentTournamentOpponent )
		{
			
			case 0:
				// Granat Göran
				enemyInstance.Name          = "Player2";
				enemyInstance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemyInstance.AddToCamera   = true;
				enemyInstance.PlayerIndex   = 1;
				enemyInstance.Health        = 3;
				enemyInstance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 1:
				// Bosse Bazooka
				enemyInstance.Name          = "Player2";
				enemyInstance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemyInstance.AddToCamera   = true;
				enemyInstance.PlayerIndex   = 1;
				enemyInstance.Health        = 3;
				enemyInstance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 2:
				// Molotov Melker
				enemyInstance.Name          = "Player2";
				enemyInstance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemyInstance.AddToCamera   = true;
				enemyInstance.PlayerIndex   = 1;
				enemyInstance.Health        = 3;
				enemyInstance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 3:
				// Örjan den överlägsna
				enemyInstance.Name          = "Player2";
				enemyInstance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemyInstance.AddToCamera   = true;
				enemyInstance.PlayerIndex   = 1;
				enemyInstance.Health        = 4;
				enemyInstance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 4:
				// Demon Dog
				enemyInstance.Name          = "Player2";
				enemyInstance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemyInstance.AddToCamera   = true;
				enemyInstance.PlayerIndex   = 1;
				enemyInstance.Health        = 5;
				enemyInstance.Position      = new Vector2( 900.0f, 100.0f );
				break;
		}
		
		// Map
		Map mapInstance = ( Map )_map.Instantiate();
		
		// Camera
		MultiTargetDynamicCamera2D camInstance = ( MultiTargetDynamicCamera2D )_camera.Instantiate();
		camInstance.Position = new Vector2( 600, 250 );
		camInstance.PositionSmoothingEnabled = true;
		camInstance.PositionSmoothingSpeed = 1.0f;
		
		// Add as children
		GetParent().CallDeferred( "add_child", camInstance );
		GetParent().CallDeferred( "add_child", mapInstance );
		GetParent().CallDeferred( "add_child", p1Instance );
		GetParent().CallDeferred( "add_child", enemyInstance );
		
		QueueFree();
	}

}
