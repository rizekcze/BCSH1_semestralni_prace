using Godot;
using System;

public partial class ControlsMenu : Control
{
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel")) // "ui_cancel"-> Esc
		{
			GetViewport().SetInputAsHandled();
			OnBackPressed();
		}
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
