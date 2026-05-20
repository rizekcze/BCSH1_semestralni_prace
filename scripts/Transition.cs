using Godot;
using System;

public partial class Transition : CanvasLayer
{
	private AnimationPlayer _anim;
	private Label _label;

	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_label = GetNode<Label>("LevelLabel");
		
		GetNode<ColorRect>("Overlay").Modulate = new Color(1, 1, 1, 0);
		_label.Modulate = new Color(1, 1, 1, 0);
	}

	public async void PlayTransition(string levelName)
	{
		_label.Text = levelName;
		_anim.Play("fade_in_out");
		
		await ToSignal(_anim, AnimationPlayer.SignalName.AnimationFinished);
	}
}
