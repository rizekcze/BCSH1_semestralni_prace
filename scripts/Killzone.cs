using Godot;
using System;

public partial class Killzone : Area2D
{
	public async void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.IsDead = true;
			player.Velocity = Vector2.Zero;

			var sprite = player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			sprite.Play("Death");

			GetNode<Sounds>("/root/Sounds").PlayDeath();

			await ToSignal(GetTree().CreateTimer(1.2), SceneTreeTimer.SignalName.Timeout);
			var gm = GetNode<GameManager>("/root/GameManager");
			gm.DeathCount++;
			player.ResetPlayer();
		}
	}
}
