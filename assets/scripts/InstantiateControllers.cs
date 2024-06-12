using Godot;
using System;

public partial class InstantiateControllers : Node
{
	private GameManager _gameManager;
	private PackedScene _player;
	private PackedScene _enemy;
	private PackedScene _map;
	private PackedScene _camera;
	private PackedScene _opponentUI;

	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>( "/root/GameManager" );
		_player      = ResourceLoader.Load<PackedScene>( "res://player_controller.tscn" );
		_map         = ResourceLoader.Load<PackedScene>( "res://map.tscn" );
		_camera      = ResourceLoader.Load<PackedScene>( "res://multi_target_dynamic_camera_2d.tscn" );
		_enemy       = ResourceLoader.Load<PackedScene>( "res://assets/scripts/Controllers/Enemies/enemy" + 
		                                                 ( _gameManager.CurrentTournamentOpponent + 1 ) +
		                                                 "_controller.tscn" );
		_opponentUI  = ResourceLoader.Load<PackedScene>( "res://assets/UI/FadeOutLabel.tscn" );

		// Player 1
		PlayerController p1Instance = ( PlayerController )_player.Instantiate();
		p1Instance.Name          = "Player1";
		p1Instance.Nickname		 = "Cool Dog";
		p1Instance.Controls      = ( PlayerControls )GD.Load( "res://assets/Resource/p1_Controls.tres" );
		p1Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Collie.png" );
		p1Instance.AddToCamera   = true;
		p1Instance.PlayerIndex   = 0;
		p1Instance.Health        = 3;
		p1Instance.Position      = new Vector2( 100.0f, 100.0f );

		// Player 2
		PlayerController p2Instance = ( PlayerController )_player.Instantiate();
		p2Instance.Name          = "Player2";
		p2Instance.Nickname		 = "Player 2";
		p2Instance.Controls      = ( PlayerControls )GD.Load( "res://assets/Resource/p2_Controls.tres" );
		p2Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
		p2Instance.AddToCamera   = true;
		p2Instance.PlayerIndex   = 1;
		p2Instance.Health        = 3;
		p2Instance.Position      = new Vector2( 900.0f, 100.0f );

		// Declare instance
		Enemy1Controller enemy1Instance = null;
		Enemy2Controller enemy2Instance = null;
		Enemy3Controller enemy3Instance = null;


		switch ( _gameManager.CurrentTournamentOpponent )
		{
			case 0:
				// Granat Göran
				enemy1Instance = ( Enemy1Controller )_enemy.Instantiate();
				enemy1Instance.Name          = "Player2";
				enemy1Instance.Nickname      = "Gränslösa Granat-Göran";
				enemy1Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemy1Instance.AddToCamera   = true;
				enemy1Instance.PlayerIndex   = 1;
				enemy1Instance.Health        = 5;
				enemy1Instance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 1:
				// Bosse Bazooka
				enemy2Instance = ( Enemy2Controller )_enemy.Instantiate();
				enemy2Instance.Name          = "Player2";
				enemy2Instance.Nickname		 = "Bångstyriga Bazooka Benny";
				enemy2Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Golden.png" );
				enemy2Instance.AddToCamera   = true;
				enemy2Instance.PlayerIndex   = 1;
				enemy2Instance.Health        = 5;
				enemy2Instance.Position      = new Vector2( 900.0f, 100.0f );
				break;
			case 2:
				// Örjan den överlägsna
				enemy3Instance = ( Enemy3Controller )_enemy.Instantiate();
				enemy3Instance.Name          = "Player2";
				enemy3Instance.Nickname		 = "Örjan den Överlägsne";
				enemy3Instance.PlayerTexture = GD.Load<CompressedTexture2D>( "res://assets/sprites/Dog_Demon.png" );
				enemy3Instance.AddToCamera   = true;
				enemy3Instance.PlayerIndex   = 1;
				enemy3Instance.Health        = 10;
				enemy3Instance.Position      = new Vector2( 900.0f, 100.0f );
				break;
		}

		// Map
		Map mapInstance = ( Map )_map.Instantiate();

		// Camera
		MultiTargetDynamicCamera2D camInstance = ( MultiTargetDynamicCamera2D )_camera.Instantiate();
		camInstance.Position                 = new Vector2( 600, 250 );
		camInstance.PositionSmoothingEnabled = true;
		camInstance.PositionSmoothingSpeed   = 1.0f;
		
		// UI
		FadeOutLabel opponentUIInstance = ( FadeOutLabel )_opponentUI.Instantiate();

		// Add as children
		GetParent().CallDeferred( "add_child", camInstance );
		GetParent().CallDeferred( "add_child", mapInstance );
		GetParent().CallDeferred( "add_child", p1Instance );

		switch ( _gameManager.GameMode )
		{
			case GameManager.EGameMode.PvP:
				GetParent().CallDeferred( "add_child", p2Instance );
				break;
			case GameManager.EGameMode.PvAISingle:
				GetParent().CallDeferred( "add_child", enemy1Instance );
				break;
			case GameManager.EGameMode.PvAITournament:
				switch ( _gameManager.CurrentTournamentOpponent )
				{
					case 0:
						GetParent().CallDeferred( "add_child", enemy1Instance );
						break;
					case 1:
						GetParent().CallDeferred( "add_child", enemy2Instance );
						break;
					case 2:
						GetParent().CallDeferred( "add_child", enemy3Instance );
						break;
				}
				break;
		}
		GetParent().CallDeferred( "add_child", opponentUIInstance );

		QueueFree();
	}
}
