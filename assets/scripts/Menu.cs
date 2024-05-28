using Godot;
using System;

public partial class Menu : Control
{
	private PackedScene _gameScene;
	private GameManager _gameManager;
	public override void _Ready()
	{
		_gameScene = ResourceLoader.Load<PackedScene>( "res://main.tscn" );
		_gameManager = GetNode<GameManager>("/root/GameManager");
	}
	public void _on_player_vs_player_pressed()
	{
		_gameManager.GameMode = GameManager.EGameMode.PvP;
		GetTree().ChangeSceneToPacked( _gameScene );
	}
	public void _on_player_vs_ai_pressed()
	{
		_gameManager.GameMode = GameManager.EGameMode.PvAi;
		GetTree().ChangeSceneToPacked( _gameScene );
	}
	public void _on_exit_game_pressed()
	{
		GetTree().Quit();
	}
}
