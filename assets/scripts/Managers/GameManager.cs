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
		ReloadMap();
	}
	private void ReloadMap()
	{
		GetTree().ChangeSceneToPacked( ResourceLoader.Load<PackedScene>( "res://main.tscn" ) );
	}
}
