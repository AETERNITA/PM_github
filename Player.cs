using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export] private TextureProgressBar _healthbar;
	[Export] private AnimatedSprite2D _animatedSprite; // Reference to AnimatedSprite2D

	public const float Speed = 400.0f;
	public const float JumpVelocity = -500.0f;
	private int jumpCount = 0;
	private double jumpTimer = 0.3;
	private bool jumpActive = false;

	[Export] private Node2D gunSprite;

	public override void _Ready()
	{
		if (_animatedSprite == null)
		{
			GD.PrintErr("ERROR: _animatedSprite is not assigned in the Inspector!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			jumpCount = 0;
			velocity.Y = -2; // Small downward force to prevent jittering
		}

		// Handle Jump
		if (jumpActive && jumpTimer <= 0)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
			jumpActive = false;
			_animatedSprite.Play("Jump"); // Play jump animation
		}

		if (Input.IsActionJustPressed("w") && jumpCount == 1)
		{
			jumpActive = true;
			jumpCount = 2;
		}

		if (Input.IsActionJustPressed("w") && jumpCount < 2 && jumpTimer <= 0)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
			_animatedSprite.Play("Jump"); // Play jump animation
		}

		// Handle Left/Right Movement
		Vector2 direction = Input.GetVector("a", "d", "ui_up", "ui_down");
		if (direction.X != 0)
		{
			velocity.X = direction.X * Speed;
			
			// Flip sprite based on movement direction
			_animatedSprite.FlipH = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed * (float)delta);
		}

		// If player is on the ground and no input, play idle animation
		if (IsOnFloor())
		{
			if (velocity.X == 0)
			{
				_animatedSprite.Play("Idle");
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		if (jumpTimer > 0)
		{
			jumpTimer -= delta;
		}

		RotateGunToMouse();
	}

	private void RotateGunToMouse()
	{
		if (gunSprite != null)
		{
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 directionToMouse = mousePosition - gunSprite.GlobalPosition;
			float angle = directionToMouse.Angle();
			gunSprite.Rotation = angle;
		}
	}
}
