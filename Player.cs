using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private TextureProgressBar _healthbar;

	public const float Speed = 400.0f;
	public const float JumpVelocity = -500.0f;
	private int jumpCount = 0;
	private double jumpTimer = 0.3;
	private bool jumpActive = false;

	// Reference to the gun sprite (child of the Node2D)
	[Export] private Node2D gunSprite;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			jumpCount = 0;
			velocity = new Vector2(0, -2);
		}

		// Handle Jump.
		if (jumpActive && jumpTimer <= 0)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
			jumpActive = false;
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
		}

		// Get input direction and handle movement.
		Vector2 direction = Input.GetVector("a", "d", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
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

		// Rotate the gun to face the mouse position.
		RotateGunToMouse();
	}

	private void RotateGunToMouse()
	{
		if (gunSprite != null)
		{
			// Get the position of the mouse relative to the player.
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 directionToMouse = mousePosition - gunSprite.GlobalPosition;

			// Calculate the angle to the mouse.
			float angle = directionToMouse.Angle();

			// Apply the rotation to the gun sprite.
			gunSprite.Rotation = angle;
		}
	}

	public void _on_heal_button_pressed()
	{
		_healthbar.Value += 10;
		if (_healthbar.Value > 100) _healthbar.Value = 100;
	}

	public void _on_damage_button_pressed()
	{
		_healthbar.Value -= 10;
		if (_healthbar.Value < 0) _healthbar.Value = 0;
	}
}
