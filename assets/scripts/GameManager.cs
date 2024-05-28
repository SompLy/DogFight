using Godot;
using System;

public partial class GameManager : Node
{
	public enum EGameMode
	{
		PvP,
		PvAi
	}

	public EGameMode GameMode = EGameMode.PvP;
	
	public bool UseMouse = false;
	public int CurrentRound = 0;

	public override void _Process(double delta)
	{
		GD.Print( GameMode );
	}
}
