using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export] private TextureProgressBar _healthbar;
	[Export] private AnimatedSprite2D _animatedSprite; // Reference to AnimatedSprite2D
	[Export] private Node2D gunSprite;
	private AudioStreamPlayer Move;

	public const float Speed = 400.0f;
	public const float JumpVelocity = -500.0f;
	private int jumpCount = 0;
	private double jumpTimer = 0.3;
	private bool jumpActive = false;

//Inventory Variables; Effects are also Items
	//Dynamic Variables
	List<string> Item = new List<string>();
	List<double> Item_Strenght = new List<double>();
	List<double> Item_durabillity = new List<double>();
	List<double> Item_duration = new List<double>();
	//"static" variables
	Dictionary<string, string> initial_effect = new Dictionary<string, string>();
	Dictionary<string, string> continuous_effect = new Dictionary<string, string>();
	Dictionary<string, string> end_efect = new Dictionary<string, string>();
	

	public override void _Ready()
	{
			initialise_inventory_system();

		if (_animatedSprite == null)
		{
			GD.PrintErr("ERROR: _animatedSprite is not assigned in the Inspector!");
		}
		Move = GetNode<AudioStreamPlayer>("Move");
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
			Move.Play();
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
		

		Update_Inventory(delta);
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

	private void Update_Inventory(double delta_time)
	{
		int i = 0;
		foreach (string element in Item)
		{
			if (Item_duration[i] <= delta_time && !(Item_duration[i] == -1))
			{
				Item_Effect("end", Item[i]);
				Item.Remove(Item[i]);
				Item_durabillity.Remove(Item_duration[i]);
				Item_duration.Remove(Item_duration[i]);
				Item_Strenght.Remove(Item_Strenght[i]);
			}
			else
			{
				Item_duration[i] = Item_duration[i] - delta_time;
			}

			i = i++;
		}
	}

	private void Item_add(string item_name, double item_strenght, double item_durabillity, double item_duration)
	{
		Item.Add(item_name);
		Item_Strenght.Add(item_strenght);
		Item_durabillity.Add(item_durabillity);
		Item_duration.Add(item_duration);
	}

	private void Item_Effect (string Time, string Item)
	{
		switch (Time)
		{
			case "initial":
			Item_initial_effect(Item);
			break;

			case "continuous":
			Item_continuous_effect(Item);
			break;

			case "end":
			Item_end_effect(Item);
			break;

			default:
			break;
		}
	}

	// THE CODE FOR THE EFFECTS AND THE ITEMS IS IN THE SWITCH STATEMENTS BELOW
	// add variables for conditions affecting other systems at the top

	private void Item_initial_effect (string Item)
	{
		switch (initial_effect[Item])
			{
				default:
				break;
			}
	}

	private void Item_continuous_effect (string Item)
	{
		switch (continuous_effect[Item])
			{
				default:
				break;
			}
	}

	private void Item_end_effect (string Item)
	{
		switch (end_efect[Item])
			{
				default:
				break;
			}
	}

	//ADD THE CODE FOR INITIALISING THE DICTIONARIES HERE:

	private void initialise_inventory_system ()
	{
		//initial_effect.Add()
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

	public void _on_area_2d_body_entered(Node2D body)
	{
   		//add item 
		GD.Print("item used");
	}

	 
}
