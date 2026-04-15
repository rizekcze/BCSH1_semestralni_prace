using Godot;
using System;

public partial class FinishLine : Area2D
{
	private bool _canInteract = false;
	private GameManager _gm;

	public override void _Ready()
	{
		_gm = GetNode<GameManager>("/root/GameManager");
	}

	public override void _Process(double delta)
	{
		// Iteract E pouze, pokud je hráč v Area2D
		if (_canInteract && Input.IsActionJustPressed("Interact"))
		{
			GoToNextLevel();
		}
	}

	private void GoToNextLevel()
	{
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.GoToNextLevelWithTransition();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_canInteract = true;
			GD.Print("Stiskni E pro další level");
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			_canInteract = false;
		}
	}
}
