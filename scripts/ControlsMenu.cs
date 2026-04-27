using Godot;
using System;

public partial class ControlsMenu : Control
{
	// Tato metoda se zavolá při jakémkoliv vstupu
	public override void _Input(InputEvent @event)
	{
		// Kontrola, zda byla stisknuta klávesa Esc
		if (@event.IsActionPressed("ui_cancel")) // "ui_cancel" je standardně namapováno na Esc
		{
			// Označíme vstup za zpracovaný, aby ho nezachytila jiná menu (třeba PauseMenu)
			GetViewport().SetInputAsHandled();
			OnBackPressed();
		}
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
