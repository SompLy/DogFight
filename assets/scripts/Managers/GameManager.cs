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

	public override void _Input( InputEvent @event )
	{
		if ( @event is InputEventKey { Keycode: Key.M, Pressed: true } )
		{
			UseMouse = !UseMouse;
		}
	}
}
