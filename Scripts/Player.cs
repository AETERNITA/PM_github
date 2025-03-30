using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;


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
	List<int> ForDeletion = new List<int>();
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

		Update_Inventory(delta);

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
			if ((Item_duration[i] <= delta_time && !(Item_duration[i] == -1)) || (Item_durabillity[i] <= 0 && !((Item_durabillity[i] == -1))))
			{
				//Print("if");
				Item_Effect("end", Item[i], Item_Strenght[i]);
				Item.Remove(Item[i]);
				Item_durabillity.Remove(Item_duration[i]);
				Item_duration.Remove(Item_duration[i]);
				Item_Strenght.Remove(Item_Strenght[i]);
				break;
				//ForDeletion.Add(i);
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

	public void _on_area_2d_body_entered(Node2D body)
	{
   		//add item 
		GD.Print("item used");
	}






}
