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


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		} else {
			jumpCount = 0; 
			velocity = new Vector2(0, -2);
		} 

		// Handle Jump.
		if(jumpActive && jumpTimer <= 0){
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
			jumpActive = false;
		}
		if (Input.IsActionJustPressed("w") && jumpCount == 1){
			jumpActive = true;
			jumpCount = 2;
		}
		if (Input.IsActionJustPressed("w") && jumpCount < 2 && jumpTimer <= 0)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
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

	public override void _Process(double delta){
		if(jumpTimer > 0){
			jumpTimer -= delta;
		}

		
	}

	public void _on_heal_button_pressed()
	{
		{
			_healthbar.Value += 10;
			if (_healthbar.Value >100 ) _healthbar.Value = 100; 
		}
	}

	public void _on_damage_button_pressed()
	{
		{
			_healthbar.Value -= 10;
			if (_healthbar.Value < 0 ) _healthbar.Value = 0; 
		}
	}


}
