using Godot;
using System;

public partial class GameManager : Node
{
	public enum EGameMode
	{
		PvP,
		PvAISingle,
		PvAITournament
	}
	
	public EGameMode GameMode = EGameMode.PvP;
	
	public bool UseMouse = false;
	public int CurrentRound = 0;
	public int CurrentTournamentOpponent = 0;
	public int Player1Score = 0;
	public int Player2Score = 0;
	
	public override void _Input( InputEvent @event )
	{
		if ( @event is InputEventKey { Keycode: Key.M, Pressed: true } )
		{
			UseMouse = !UseMouse;
		}
		if ( @event is InputEventKey { Keycode: Key.Escape, Pressed: true } )
		{
			LoadMap( "res://main_menu.tscn" );
			Player1Score = 0;
			Player2Score = 0;
			CurrentRound = 0;
		}
	}

	public void PlayerDeath( int playerIndex )
	{
		switch ( playerIndex )
		{
			case 0:
				Player2Score++;
				break;
			case 1:
				Player1Score++;
				break;
		}


		if ( GameMode == EGameMode.PvAITournament )
		{
			if ( Player1Score == 3 )
			{
				CurrentTournamentOpponent++;
				Player1Score = 0;
				Player2Score = 0;
			}

			if ( Player2Score == 3 )
			{
				LoadMap( "res://main_menu.tscn" );
				Player1Score = 0;
				Player2Score = 0;
			}
		}

		CurrentRound++;
		LoadMap( "res://main.tscn" );
		
	}
	private void LoadMap( string path )
	{
		GetTree().ChangeSceneToPacked( ResourceLoader.Load<PackedScene>( path ) );
	}
}
