using Godot;
using System;

public partial class Menu : Control
{
	private PackedScene _gameScene;

	public override void _Ready()
	{
		_gameScene = ResourceLoader.Load<PackedScene>( "res://main.tscn" );
	}

	public void _on_player_vs_player_pressed()
	{
		GetTree().ChangeSceneToPacked( _gameScene );
	}
	public void _on_player_vs_ai_pressed()
	{
		
	}
	public void _on_exit_game_pressed()
	{
		GetTree().Quit();
	}
}
