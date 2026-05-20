using Godot;
using System;

public partial class Coin : Area2D
{
	public override void _Ready()
{
	var gm = GetNode<GameManager>("/root/GameManager");
	
	if (gm.CollectedCoinsIDs.Count == 0) 
	{
		gm.LoadGame();
	}

	string uniqueId = GetTree().CurrentScene.Name + "_" + Name;

	if (gm.CollectedCoinsIDs.Contains(uniqueId))
	{
		QueueFree();
	}
}

	private void OnBodyEntered(Node2D body)
{
	if (body is Player)
	{
		var gm = GetNode<GameManager>("/root/GameManager");
		string uniqueId = GetTree().CurrentScene.Name + "_" + Name;

		if (!gm.CollectedCoinsIDs.Contains(uniqueId))
		{
			gm.CollectedCoinsIDs.Add(uniqueId);
			gm.TotalCoins++;
			gm.SaveGame();
		}

		GetNode<Sounds>("/root/Sounds").PlayCoin();

		QueueFree();
	}
}
}
