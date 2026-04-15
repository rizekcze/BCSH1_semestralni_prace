using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class GameManager : Node
{
	public enum GameLevel { Level1, Level2, Level3, Menu }

	private readonly System.Collections.Generic.Dictionary<GameLevel, string> _levelPaths =
		new System.Collections.Generic.Dictionary<GameLevel, string>
	{
		{ GameLevel.Level1, "res://scenes/game_level1.tscn" },
		{ GameLevel.Level2, "res://scenes/game_level2.tscn" },
		{ GameLevel.Level3, "res://scenes/game_level3.tscn" },
		{ GameLevel.Menu, "res://scenes/main_menu.tscn"}
	};

	public int TotalCoins = 0;
	public int DeathCount = 0;
	public GameLevel CurrentLevel = GameLevel.Level1;
	public List<string> CollectedCoinsIDs = new List<string>();
	public Vector2 LastPlayerPosition = Vector2.Zero;

	private string _savePath = "user://savegame.json";

	public override void _Ready()
	{
		LoadGame();
	}

	// Volá se při stisku New Game v Menu
	public void NewGame()
	{
		TotalCoins = 0;
		DeathCount = 0;
		CurrentLevel = GameLevel.Level1;
		CollectedCoinsIDs.Clear();
		LastPlayerPosition = Vector2.Zero;
		SaveGame();

		ChangeSceneWithTransition();
	}

	// --- KLÍČOVÁ METODA PRO PŘECHOD MEZI LEVELY ---
	public async void GoToNextLevelWithTransition()
	{
		var transition = GetNode<Transition>("/root/Transition");

		// 1. Logika: Co bude další level?
		GameLevel nextLevel = CurrentLevel switch
		{
			GameLevel.Level1 => GameLevel.Level2,
			GameLevel.Level2 => GameLevel.Level3,
			_ => GameLevel.Menu
		};

		// 2. Příprava textu pro animaci
		string text = nextLevel == GameLevel.Menu ? "Thanks for playing!" : "Level " + ((int)nextLevel + 1);
		transition.PlayTransition(text);

		// 3. Čekáme na úplné zatmění (1 vteřina podle tvého timeru)
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);

		// 4. Update dat v paměti
		CurrentLevel = nextLevel;
		LastPlayerPosition = Vector2.Zero; // Resetujeme pozici, aby hráč v novém levelu nezačal v cíli

		// Pokud jsme dohráli, připravíme save na Level 1 pro příště, ale načteme Menu
		if (CurrentLevel == GameLevel.Menu)
		{
			CurrentLevel = GameLevel.Level1;
			SaveGame();
			GetTree().ChangeSceneToFile(_levelPaths[GameLevel.Menu]);
		}
		else
		{
			SaveGame();
			GetTree().ChangeSceneToFile(_levelPaths[CurrentLevel]);
		}
	}

	// Obecná metoda pro změnu scény (pro New Game nebo Load)
	public async void ChangeSceneWithTransition()
	{
		var transition = GetNode<Transition>("/root/Transition");
		string text = CurrentLevel == GameLevel.Menu ? "Main Menu" : "Level " + ((int)CurrentLevel + 1);

		transition.PlayTransition(text);
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);

		GetTree().ChangeSceneToFile(_levelPaths[CurrentLevel]);
	}

	public void SaveGame()
	{
		var godotArray = new Godot.Collections.Array();
		foreach (var id in CollectedCoinsIDs) godotArray.Add(id);

		var data = new Godot.Collections.Dictionary
		{
			{ "TotalCoins", TotalCoins },
			{ "DeathCount", DeathCount },
			{ "CurrentLevel", (int)CurrentLevel },
			{ "CollectedCoins", godotArray },
			{ "PlayerPosX", LastPlayerPosition.X },
			{ "PlayerPosY", LastPlayerPosition.Y }
		};

		using var file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Write);
		if (file != null) file.StoreLine(Json.Stringify(data));
	}

	public void LoadGame()
	{
		if (!FileAccess.FileExists(_savePath)) return;

		using var file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Read);
		var json = new Json();
		if (json.Parse(file.GetLine()) == Error.Ok)
		{
			var data = (Godot.Collections.Dictionary)json.Data;
			TotalCoins = (int)data["TotalCoins"];
			DeathCount = (int)data["DeathCount"];
			CurrentLevel = (GameLevel)(int)data["CurrentLevel"];

			var coins = (Godot.Collections.Array)data["CollectedCoins"];
			CollectedCoinsIDs.Clear();
			foreach (Variant c in coins) CollectedCoinsIDs.Add(c.AsString());

			if (data.ContainsKey("PlayerPosX"))
			{
				LastPlayerPosition = new Vector2((float)data["PlayerPosX"], (float)data["PlayerPosY"]);
			}
		}
	}


	
}
