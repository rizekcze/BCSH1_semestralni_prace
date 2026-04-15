using Godot;
using System;

public static class SaveManager
{
	private const string SavePath = "user://savegame.dat";

	// Data, která chceme ukládat
	public static int CurrentLevel = 1;
	public static int GemsCount = 0;
	public static int DeathCount = 0;

	public static void SaveGame()
	{
		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
		if (file != null)
		{
			file.StoreLine(CurrentLevel.ToString());
			file.StoreLine(GemsCount.ToString());
			file.StoreLine(DeathCount.ToString());
			GD.Print("Hra uložena do: " + OS.GetUserDataDir());
		}
	}

	public static void LoadGame()
	{
		if (!FileAccess.FileExists(SavePath)) return;

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
		if (file != null)
		{
			CurrentLevel = int.Parse(file.GetLine());
			GemsCount = int.Parse(file.GetLine());
			DeathCount = int.Parse(file.GetLine());
			GD.Print("Hra načtena.");
		}
	}
}
