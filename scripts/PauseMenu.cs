using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	private Label _statsLabel;
	private GameManager _gm;

	public override void _Ready()
	{
		_gm = GetNode<GameManager>("/root/GameManager");
		_statsLabel = GetNode<Label>("VBoxContainer/StatsLabel"); 
		
		Hide(); 
		ProcessMode = ProcessModeEnum.Always; 
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel")) 
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (GetTree().CurrentScene.Name == "MainMenu") return;
		
		bool newPauseState = !GetTree().Paused;
		GetTree().Paused = newPauseState;
		
		if (newPauseState)
		{
			_statsLabel.Text = $"Coins: {_gm.TotalCoins} | Deaths: {_gm.DeathCount}";
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void OnResume()
	{
		TogglePause();
	}

	private void OnMenu()
	{
		var player = GetTree().CurrentScene.GetNodeOrNull<CharacterBody2D>("Player");

		if (player != null)
		{
			_gm.LastPlayerPosition = player.GlobalPosition;
			GD.Print($"Pozice uložena: {player.GlobalPosition}");
		}

		_gm.SaveGame();

		GetTree().Paused = false; 
		Hide();
		
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
