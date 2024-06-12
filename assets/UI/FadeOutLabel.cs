using Godot;
using System;

public partial class FadeOutLabel : Control
{
	private Label _label;
	private GameManager _gameManager;
	float _alpha = 1.0f;
	public override void _Ready()
	{
		_label = GetNode<Label>( "Label" );
		_gameManager = GetNode<GameManager>( "/root/GameManager" );
		if ( _gameManager.GameMode == GameManager.EGameMode.PvP )
			_label.Text = "Dog Vs. Dawg" + "\n" + _gameManager.Player1Score + " | " + _gameManager.Player2Score;
		else
			_label.Text = GetParent().GetNode<Controller>( "Player1" ).Nickname + 
			              " Vs. " + GetParent().GetNode<Controller>( "Player2" ).Nickname + "\n" + 
		              _gameManager.Player1Score + " | " + _gameManager.Player2Score;
	}

	public override void _Process(double delta)
	{
		_alpha -= 0.2f * ( float )delta;
		_label.AddThemeColorOverride( "font_color", new Color( 1, 1, 1, _alpha ) );
	}
}
