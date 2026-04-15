using Godot;
using System;

public partial class Sounds : Node
{
	public override void _Ready()
	{
		GetNode<AudioStreamPlayer>("Music").Play();
	}

	public void PlayDeath() => GetNode<AudioStreamPlayer>("Death").Play();
	public void PlayCoin() => GetNode<AudioStreamPlayer>("Coin").Play();
	public void PlaySlimeSlash() => GetNode<AudioStreamPlayer>("SlimeSlash").Play();
	public void PlayJump()
	{
		var player = GetNode<AudioStreamPlayer>("Jump");
		// Randomizer skoku
		player.PitchScale = (float)GD.RandRange(0.9, 1.1);
		player.Play();
	}
}
