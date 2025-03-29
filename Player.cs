using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;


public partial class Player : CharacterBody2D
{
	[Export] private TextureProgressBar _healthbar;

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
	List<int> ForDeletion = new List<int>();
	//"static" variables
	Dictionary<string, string> initial_effect = new Dictionary<string, string>();
	Dictionary<string, string> continuous_effect = new Dictionary<string, string>();
	Dictionary<string, string> end_efect = new Dictionary<string, string>();

	// Reference to the gun sprite (child of the Node2D)
	[Export] private Node2D gunSprite;


	public override void _Ready ()
	{
		initialise_inventory_system();
	}

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

		Update_Inventory(delta);
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

	private void Update_Inventory(double delta_time)
	{
		int i = 0;
		foreach (string element in Item)
		{
			if ((Item_duration[i] <= delta_time && !(Item_duration[i] == -1)) || (Item_durabillity[i] <= 0 && !((Item_durabillity[i] == -1))))
			{
				//Print("if");
				Item_Effect("end", Item[i], Item_Strenght[i]);
				Item.Remove(Item[i]);
				Item_durabillity.Remove(Item_duration[i]);
				Item_duration.Remove(Item_duration[i]);
				Item_Strenght.Remove(Item_Strenght[i]);
				ForDeletion.Add(i);
			}
			else
			{
				Item_duration[i] = Item_duration[i] - delta_time;
				Item_Effect("continuous", Item[i], Item_Strenght[i]);
			}

			i = i++;
			
		}
		//foreach (int k in ForDeletion)
		//{
		//	Item.Remove(Item[k]);
		//	Item_durabillity.Remove(Item_duration[k]);
		//	Item_duration.Remove(Item_duration[k]);
		//	Item_Strenght.Remove(Item_Strenght[k]);
		//}
	}

	private void Item_add(string item_name, double item_strenght, double item_durabillity, double item_duration)
	{
		Item.Add(item_name);
		Item_Strenght.Add(item_strenght);
		Item_durabillity.Add(item_durabillity);
		Item_duration.Add(item_duration);
		//Print(Item[0]);
		//Print(Item_durabillity[0]);
		//Print(Item_Strenght[0]);
		//Print(Item_duration[0]);
	}



	private void Item_Effect (string Time, string Item, double Item_Strenght)
	{
		//Print("effect");
		switch (Time)
		{
			case "initial":
			Item_initial_effect(Item, Item_Strenght);
			break;

			case "continuous":
			Item_continuous_effect(Item, Item_Strenght);
			break;

			case "end":
			Item_end_effect(Item, Item_Strenght);
			break;

			default:
			break;
		}
	}

	// THE CODE FOR THE EFFECTS AND THE ITEMS IS IN THE SWITCH STATEMENTS BELOW
	// add variables for conditions affecting other systems at the top

	private void Item_initial_effect (string Item, double strenght)
	{
		Print("initial_effect");
		switch (initial_effect[Item])
			{
				case "instant_healing":
				_healthbar.Value += strenght;
				break;

				case "nothing":
				break;
				
				default:
				Print("Warning: initial effect: default case triggered");
				break;
			}
	}

	private void Item_continuous_effect (string Item, double strenght)
	{
		//Print("continouos_effect");
		switch (continuous_effect[Item])
			{
				case "regeneration":
				_healthbar.Value += strenght;
				if (_healthbar.Value > 100) _healthbar.Value = 100;
				break;

				case "nothing":
				break;
				
				default:
				Print("Warning: continouos effect: default case triggered");
				break;
			}
	}

	private void Item_end_effect (string Item, double strenght )
	{
		Print("end_effect");
		switch (end_efect[Item])
			{
				case "nothing":
				break;
				
				default:
				Print("Warning: end effect: default case triggered");
				break;
			}
	}
    
	//ADD THE CODE FOR INITIALISING THE DICTIONARIES HERE:

	private void initialise_inventory_system ()
	{		
		initial_effect.Add("healing_effect", "instant_healing");
		continuous_effect.Add("healing_effect", "regeneration");
		end_efect.Add("healing_effect", "nothing");


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
		Item_add("healing_effect", 1, -1, 0.75);
		//Print("item added");
		
		//minderwertiger alter code
		//_healthbar.Value += 10;
		//if (_healthbar.Value > 100) _healthbar.Value = 100;
	}

	public void _on_damage_button_pressed()
	{
		_healthbar.Value -= 10;
		if (_healthbar.Value < 0) _healthbar.Value = 0;
		
	}







}