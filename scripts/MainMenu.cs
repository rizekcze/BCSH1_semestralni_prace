using Godot;
using System;

public partial class MainMenu : Control
{
	private GameManager _gm;

	public override void _Ready()
	{
		_gm = GetNode<GameManager>("/root/GameManager");
		_gm.LoadGame();
	}

	private void OnNewGame()
	{
		_gm.NewGame();
		_gm.ChangeSceneWithTransition();
	}

	private void OnLoadGame()
	{
		_gm.ChangeSceneWithTransition();
	}

	private void OnControls()
	{
		GetTree().ChangeSceneToFile("res://scenes/controls_menu.tscn");
	}

	private void OnExit()
	{
		GetTree().Quit();
	}
}
