using Godot;
using System;

public partial class MainMenu : Control
{
	private GameManager _gm;

	public override void _Ready()
	{
		// GameManager Autoload
		_gm = GetNode<GameManager>("/root/GameManager");
		_gm.LoadGame();
	}

	// Tlačítko: New Game
	private void OnNewGame()
	{
		_gm.NewGame();
		_gm.ChangeSceneWithTransition();
	}

	// Tlačítko: Load Game 
	private void OnLoadGame()
	{
		_gm.ChangeSceneWithTransition();
	}

	// Tlačítko: Controls (např. vyskočí panel nebo nová scéna)
	private void OnControls()
	{
		GetTree().ChangeSceneToFile("res://scenes/controls_menu.tscn");
	}

	// Tlačítko: Exit
	private void OnExit()
	{
		GetTree().Quit();
	}
}
