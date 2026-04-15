using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 50.0f;
	private int _direction = 1;

	private AnimatedSprite2D _sprite;
	private RayCast2D _sensor;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sensor = GetNode<RayCast2D>("Sensor");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (_sensor.IsColliding())
		{
			var collider = _sensor.GetCollider();

			float distance = _sensor.GlobalPosition.DistanceTo(_sensor.GetCollisionPoint());

			if (collider is Player && distance <= 80.0f)
			{
				if (_sprite.Animation != "Attack")
				{
					_sprite.Play("Attack");

					var sounds = GetNode<Sounds>("/root/Sounds");
					sounds.PlaySlimeSlash();
				}
				velocity.X = _direction * (Speed * 1.5f);
			}

			else if (!(collider is Player) && distance <= 10.0f)
			{
				TurnAround();
			}
			else
			{
				NormalPatrol(ref velocity);
			}
		}
		else
		{
			NormalPatrol(ref velocity);
		}

		_sprite.FlipH = (_direction == -1);
		Velocity = velocity;
		MoveAndSlide();
	}

	private void NormalPatrol(ref Vector2 velocity)
	{
		_sprite.Play("Patrol");
		velocity.X = _direction * Speed;
	}

	private void TurnAround()
	{
		_direction *= -1;
		_sensor.TargetPosition = new Vector2(Mathf.Abs(_sensor.TargetPosition.X) * _direction, 0);
	}
}
