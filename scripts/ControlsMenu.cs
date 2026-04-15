using Godot;
using System;

public partial class ControlsMenu : Control
{
	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");

	}
}
