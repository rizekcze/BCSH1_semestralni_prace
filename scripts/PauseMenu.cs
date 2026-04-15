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

	// --- UPRAVENÁ METODA PRO MENU ---
	private void OnMenu()
	{
		// 1. Najdeme hráče v aktuální scéně, abychom zjistili jeho pozici
		var player = GetTree().CurrentScene.GetNodeOrNull<CharacterBody2D>("Player");

		if (player != null)
		{
			// 2. Uložíme aktuální souřadnice do GameManageru
			_gm.LastPlayerPosition = player.GlobalPosition;
			GD.Print($"Pozice uložena: {player.GlobalPosition}");
		}

		// 3. Uložíme celý stav hry do JSONu
		_gm.SaveGame();

		// 4. Resetujeme pauzu a jdeme do menu
		GetTree().Paused = false; 
		Hide();
		
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
