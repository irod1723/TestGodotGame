using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	// Basically the constructor
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("moveRight"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("moveLeft"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("moveDown"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("moveUp"))
		{
			velocity.Y -= 1;
		}

		AnimatedSprite2D PlayerAnimatedSprite = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			PlayerAnimatedSprite.Play();
		}
		else
		{
			PlayerAnimatedSprite.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			PlayerAnimatedSprite.Animation = "walk";
			PlayerAnimatedSprite.FlipV = false;
			PlayerAnimatedSprite.FlipH = velocity.X < 0; //remember that this line evaluates to a boolean
		}
		else if (velocity.Y != 0)
		{
			PlayerAnimatedSprite.Animation = "up";
			PlayerAnimatedSprite.FlipV = velocity.Y > 0;
		}
	}
	private void _on_body_entered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("PlayerHitbox").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("PlayerHitbox").Disabled = false;
	}

	[Export]
	public int Speed { get; set; } = 400;
	
	public Vector2 ScreenSize;
}


