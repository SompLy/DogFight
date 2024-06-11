using Godot;
using System;

public partial class Menu : Control
{
	private PackedScene _gameScene;
	private PackedScene _player;
	private PackedScene _enemy;
	private PackedScene _map;
	private GameManager _gameManager;

	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>( "/root/GameManager" );
		_gameScene   = ResourceLoader.Load<PackedScene>( "res://main.tscn" );
		_player      = ResourceLoader.Load<PackedScene>( "res://player_controller.tscn" );
		_map         = ResourceLoader.Load<PackedScene>( "res://map.tscn" );
	}

	public void _on_player_vs_player_pressed()
	{
		_gameManager.GameMode = GameManager.EGameMode.PvP;
		_gameManager.UseMouse = false;
		GetTree().ChangeSceneToPacked( _gameScene );
	}

	public void _on_player_vs_ai_pressed()
	{
		_gameManager.GameMode = GameManager.EGameMode.PvAISingle;
		_gameManager.UseMouse = true;
		GetTree().ChangeSceneToPacked( _gameScene );
	}

	public void _on_player_vs_ai_tournament_pressed()
	{
		_gameManager.GameMode = GameManager.EGameMode.PvAITournament;
		_gameManager.UseMouse = true;
		GetTree().ChangeSceneToPacked( _gameScene );
	}

	public void _on_exit_game_pressed()
	{
		GetTree().Quit();
	}
}
