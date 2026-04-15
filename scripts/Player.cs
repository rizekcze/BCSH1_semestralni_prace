using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -300.0f;

	public bool IsDead = false;
	private int _coins = 0;
	private Vector2 _startPosition;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_startPosition = GlobalPosition;

		var gm = GetNode<GameManager>("/root/GameManager");
		// Pokud máme uloženou pozici z minula, teleportujeme hráče
		if (gm.LastPlayerPosition != Vector2.Zero)
		{
			GlobalPosition = gm.LastPlayerPosition;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsDead) return;

		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;

			GetNode<Sounds>("/root/Sounds").PlayJump();
		}

		bool isTurboPressed = Input.IsActionPressed("Rush") &&
							 (Input.IsActionPressed("Move_left") ||
							  Input.IsActionPressed("Move_right"));

		float currentSpeed = isTurboPressed ? Speed * 1.5f : Speed;
		Vector2 direction = Input.GetVector("Move_left", "Move_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * currentSpeed;
			_animatedSprite.FlipH = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
		}

		if (IsOnFloor())
		{
			if (direction.X == 0) _animatedSprite.Play("Idle");
			else _animatedSprite.Play(isTurboPressed ? "Run" : "Walk");
		}
		else
		{
			_animatedSprite.Play(velocity.Y < 0 ? "Jump" : "Fall");
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void AddCoin()
	{
		_coins++;
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.TotalCoins++;
	}

	public void ResetPlayer()
	{
		GlobalPosition = _startPosition;
		Velocity = Vector2.Zero;
		IsDead = false;
		_animatedSprite.Play("Idle");
		// V momentě smrti:
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.SaveGame(); // Uložíme hned po smrti
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("RESET"))
		{
			ResetPlayer();
		}
	}
}
