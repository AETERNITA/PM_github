// Autoren: Dominik von Ehrenstein, Leo Appel, Yannik Duplitzer, Leander
using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;


public partial class Player : GenericCharacterClass
{
	[Export] private TextureProgressBar _healthbar;
	[Export] private AnimatedSprite2D _animatedSprite; // Reference to AnimatedSprite2D
	[Export] private Node2D gunSprite;
	private CanvasModulate canvmod;
	private bool isplaying = false;
	public float dashSpeed = 1200.0f; // Geschwindigkeit beim Dash
	public float dashTime = 0.2f; // Dauer des Dashs
	public float dashDuration = 0.2f; // Dauer des Dashs
	private bool isDashing = false;
	private bool canDash = true;
	private bool isdowndashing = false;
	private float downdash_speed = 1200.0f;
	private Vector2 dashDirection = Vector2.Zero;
	private Vector2 direction = Vector2.Zero;
	private AudioStreamPlayer Move;
	private AudioStreamPlayer Jump;
	private AudioStreamPlayer Damage;
	private AudioStreamPlayer Heal;
	private AudioStreamPlayer Heal_low_health;
	private AudioStreamPlayer Dash;
	private AudioStreamPlayer JumpBoost;
	private AudioStreamPlayer NormalSoundscape;
	private AudioStreamPlayer DownDashImpactSFX;
	private AudioStreamPlayer Landing_sfx;
	private AudioStreamPlayer Losing_sfx;
	private Camera2D PlayerCam;
	private GpuParticles2D damage_particles;
	private GpuParticles2D low_health_particles;
	private GpuParticles2D healing_particles;
	public double screenshake_duration = 0.5;
	public double screenshake_strenght_dynamic = 0;
	private double red_effect_time = 0;

	private List<string> itemqueue = new List<string>();

	private List<GenericCharacterClass> downdash_area = new List<GenericCharacterClass>();
	private List<GenericCharacterClass> laserschwert_area = new List<GenericCharacterClass>();

	public string soundscapes = "normal";

	public bool escape_menu_active = false;

	public float Speed = 400.0f;
	private float minSpeed = 390.0f;
	public float JumpVelocity = -700.0f;
	//private float dashTime = 0f;
	private int jumpCount = 0;
	private double jumpTimer = 0.3;
	private bool jumpActive = false;
	private bool jump_boosted = false;
	private bool healing = false;

	private double on_floor_temporal = 2;

	//Inventory Variables; Effects are also Items
	//Dynamic Variables
	List<string> Item = new List<string>();
	List<double> Item_Strenght = new List<double>();
	List<double> Item_durabillity = new List<double>();
	List<double> Item_duration = new List<double>();
	List<int> ForDeletion = new List<int>();
	//"static" variables
	Dictionary<string, string> initial_effect;
	Dictionary<string, string> continuous_effect;
	Dictionary<string, string> end_efect;

	float gravity_multiplier = 1.0f;

	private double step_queue_remaining = 0;
	private double step_queue_time = 0.2;

	public override void _Ready()
	{
		initialise_inventory_system();
		InitializeInputMap();

		if (_animatedSprite == null)
		{
			PrintErr("ERROR: _animatedSprite is not assigned in the Inspector!");
		}
		Move = GetNode<AudioStreamPlayer>("Move");
		Jump = GetNode<AudioStreamPlayer>("Jump");
		Damage = GetNode<AudioStreamPlayer>("Damage");
		Heal = GetNode<AudioStreamPlayer>("Heal");
		Heal_low_health = GetNode<AudioStreamPlayer>("Heal_low_health");
		Dash = GetNode<AudioStreamPlayer>("Dash");
		JumpBoost = GetNode<AudioStreamPlayer>("JumpBoost");
		NormalSoundscape = GetNode<AudioStreamPlayer>("NormalSoundscape");
		DownDashImpactSFX = GetNode<AudioStreamPlayer>("DownDashImpact");
		Landing_sfx = GetNode<AudioStreamPlayer>("Landing_sfx");
		Losing_sfx = GetNode<AudioStreamPlayer>("dead_loser");

		damage_particles = GetNode<GpuParticles2D>("damage_particles");
		low_health_particles = GetNode<GpuParticles2D>("low_health_particles");
		healing_particles = GetNode<GpuParticles2D>("healing_particles");

		PlayerCam = GetNode<Camera2D>("Camera2D2");

		canvmod = GetNode<CanvasModulate>("%canvmod");

		play_background();

	}

	public override void _PhysicsProcess(double delta)
	{

		bool play_impact = false;
		if (on_floor_temporal < 0.5 && IsOnFloor())
		{
			play_impact = true;
		}
		else
		{
			play_impact = false;
		}

		on_floor_temporal = on_floor_temporal * 0.9;
		if (IsOnFloor())
		{
			on_floor_temporal = on_floor_temporal + 1;
		}
		//Print(on_floor_temporal);

		Update_Inventory(delta);

		Vector2 velocity = Velocity;

		if (isDashing)
		{
			Dash.Play();
			dashTime -= (float)delta;
			if (dashTime <= 0)
			{
				isDashing = false; // Dash beenden
			}
			else
			{
				Velocity = dashDirection * dashSpeed;
				MoveAndSlide();
				return;
			}
		}

		direction = Input.GetVector("a", "d", "ui_up", "ui_down");

		// if alt + a or d --> dash
		if (Input.IsActionJustPressed("dash") && direction.X != 0 && !isDashing && canDash)
		{
			//Print("dash active");
			StartDash();
			return;
		}

		if (Input.IsActionJustReleased("dash"))
		{
			//Print("dash inactive");
			canDash = true;
		}

		// Add gravity
		if (!(IsOnFloor()))
		{
			velocity += GetGravity() * (float)delta * gravity_multiplier;
		}
		else
		{
			jumpCount = 0;
			velocity.Y = -2; // Small downward force to prevent jittering
		}

		if (on_floor_temporal < 0.5)
		{
			if (Input.IsActionJustPressed("down_dash") && !(isdowndashing))
			{
				velocity.Y = downdash_speed;
				Dash.Play();
				isdowndashing = true;
			}
		}

		if(Input.IsActionJustPressed("SwordHit"))
		{
            LaserschwertHit();
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
			Jump.Play();
		}

		if (Input.IsActionJustPressed("w") && jumpCount < 2 && jumpTimer <= 0)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
			jumpTimer = 0.3;
			_animatedSprite.Play("Jump"); // Play jump animation
			if (jump_boosted)
			{
				JumpBoost.Play();
			}
			else
			{
				Jump.Play();
			}
		}

		// Handle Left/Right Movement
		direction = Input.GetVector("a", "d", "ui_up", "ui_down");

		if (direction.X != 0)
		{
			if (on_floor_temporal > 0.5)
			{
				if (!(isplaying))
				{
					Move.Play();
					//Print("start_play_move");
					isplaying = true;
				}
			}
			else
			{
				Move.Stop();
				//Print("stop_play_move");
				isplaying = false;
			}
			velocity.X = direction.X * Speed;

			// Flip sprite based on movement direction
			_animatedSprite.FlipH = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed * (float)delta);
			Move.Stop();
			//Print("stop_play_move2");
			isplaying = false;
		}

		//Print(isplaying);

		// If player is on the ground and no input, play idle animation
		if (IsOnFloor())
		{
			if (velocity.X == 0)
			{
				_animatedSprite.Play("Idle");
			}
		}

		if (Math.Abs(velocity.X) < minSpeed && !(velocity.X == 0) && IsOnFloor())
		{
			velocity.X = 0;
			//Print("stop");
		}

		Velocity = velocity;
		MoveAndSlide();

		if (!(isdowndashing) && play_impact == true)
		{
			Landing_sfx.Play();
		}

		if (isdowndashing && IsOnFloor())
		{
			DownDashImpact();
			isdowndashing = false;
		}


	}

	public override void _Process(double delta)
	{
		if (jumpTimer > 0)
		{
			jumpTimer -= delta;
		}

		update_health_related_effects();

		RotateGunToMouse();

		/* if (Input.IsActionJustPressed("escape"))
		{
			if (escape_menu_active)
			{
				escape_menu_active = false;
			}
			else
			{
				escape_menu_active = true;
			}
		} */

		if (Input.IsActionJustPressed("use"))
		{
			if (itemqueue.Count > 0)
			{
				switch (itemqueue[0])
				{
					case "healing_effect":
						HealbyEffect();
						itemqueue.RemoveAt(0);

						break;

					case "jumpboost":
						JumpBoostbyEffect();
						itemqueue.RemoveAt(0);
						break;

					default:
						break;
				}
			}

		}

		var OverlayRef = GetNode("%overlay") as V2Overlay;
		if (itemqueue.Count == 0)
		{
			OverlayRef.override_inventory("", "");
		}
		else
		{
			if (itemqueue.Count == 1)
			{
				OverlayRef.override_inventory(itemqueue[0], "");
			}
			else
			{
				if (itemqueue.Count > 1)
				{
					OverlayRef.override_inventory(itemqueue[0], itemqueue[1]);
				}
			}
		}


		step_queue_remaining = step_queue_remaining - delta;
		if (step_queue_remaining <= 0 && isplaying)
		{
			step_queue_remaining = step_queue_time;
			Move.Play();
		}

		PlayerCam.Offset = new Vector2((float)screenshake_strenght_dynamic * (float)Randf() * (float)screenshake_duration, (float)screenshake_strenght_dynamic * (float)Randf() * (float)screenshake_duration);
		screenshake_duration = screenshake_duration - delta;
		if (screenshake_duration < 0)
		{
			screenshake_duration = 0;
		}

		if (red_effect_time > 0)
		{
			red_effect_time = red_effect_time - delta;
			if (red_effect_time < 0)
			{
				red_effect_time = 0;
			}
			(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 1);
		}
		else
		{
			(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 0);
		}

	}

	private void StartDash()
	{
		isDashing = true;
		dashTime = dashDuration;
		dashDirection = direction.Normalized();
		canDash = false; // Dash wurde benutzt
	}

	private void InitializeInputMap()
	{
		string actionName = "alt";

		if (!InputMap.HasAction(actionName))
		{
			InputEventKey altKey = new InputEventKey();
			altKey.Keycode = Key.Alt;

			InputMap.AddAction(actionName);
			InputMap.ActionAddEvent(actionName, altKey);
		}
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
				////Print("if");
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
		Item_Effect("initial", item_name, item_strenght);

		var OverlayRef = GetNode("%overlay") as V2Overlay;
		OverlayRef.AddPoints(500);

		////Print(Item[0]);
		////Print(Item_durabillity[0]);
		////Print(Item_Strenght[0]);
		////Print(Item_duration[0]);
	}



	private void Item_Effect(string Time, string Item, double Item_Strenght)
	{
		////Print("effect");
		switch (Time)
		{
			case "initial":
				//Print("Item_Effect triggered: initial effect");
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

	private void Item_initial_effect(string Item, double strenght)
	{
		//Print("initial_effect");
		switch (initial_effect[Item])
		{
			case "instant_healing":
				_healthbar.Value += strenght;
				if (_healthbar.Value < 30)
				{
					Heal_low_health.Play();
				}
				else
				{
					Heal.Play();
				}
				healing = true;
				break;

			case "jumpboost":
				//Print("jump initial effect for booooooooooooooooosting");
				JumpVelocity = JumpVelocity * (int)strenght;
				jump_boosted = true;
				break;

			case "nothing":
				break;

			default:
				PrintErr("Warning: initial effect: default case triggered");
				break;
		}
	}

	private void Item_continuous_effect(string Item, double strenght)
	{
		////Print("continouos_effect");
		switch (continuous_effect[Item])
		{
			case "regeneration":
				_healthbar.Value += strenght;
				if (_healthbar.Value > 100) _healthbar.Value = 100;
				healing = true;
				break;

			case "nothing":
				break;

			default:
				PrintErr("Warning: continouos effect: default case triggered");
				break;
		}
	}

	private void Item_end_effect(string Item, double strenght)
	{
		Print("end_effect");
		switch (end_efect[Item])
		{
			case "nothing":
				healing = false;
				break;

			case "jumpboost":
				JumpVelocity = JumpVelocity / (int)strenght;
				jump_boosted = false;
				break;

			default:
				PrintErr("Warning: end effect: default case triggered");
				break;
		}
	}


	private void initialise_inventory_system()
	{
		//initial_effect.Add("healing_effect", "instant_healing");
		//continuous_effect.Add("healing_effect", "regeneration");
		//end_efect.Add("healing_effect", "nothing");

		//initial_effect.Add("jumpboost", "jumpboost");
		//continuous_effect.Add("jumpboost", "nothing");
		//end_efect.Add("jumpboost", "jumpboost");
		initial_effect = Inventory_initialisation.return_inv_dictionaries()[0];
		continuous_effect = Inventory_initialisation.return_inv_dictionaries()[1];
		end_efect = Inventory_initialisation.return_inv_dictionaries()[2];

	}



	public void _on_heal_button_pressed()
	{
		//Item_add("healing_effect", 1, -1, 0.75);
		//Print("item added");

		//minderwertiger alter code
		//_healthbar.Value += 10;
		//if (_healthbar.Value > 100) _healthbar.Value = 100;

		//Heal.Play();
		itemqueue.Add("healing_effect");
	}

	public void _on_damage_button_pressed()
	{
		receive_damage(10);
	}

	//Item adding code below:
	//(string item_name, double item_strenght, double item_durabillity, double item_duration)

	public void _on_area_2d_body_entered(Node2D body)
	{
		//add item 
		//Print("item used");
	}

	public void _on_healing_potioned(Node2D body)
	{
		//Print("healing");
		//Item_add("healing_effect", 1, -1, 1.75);
		itemqueue.Add("healing_effect");
		var OverlayRef = GetNode("%overlay") as V2Overlay;
	}
	private void HealbyEffect()
	{
		Item_add("healing_effect", 1, -1, 1.75);
	}

	public void _on_jump_boosted(Node2D body)
	{
		//Print("jump boosted");
		//Item_add("jumpboost", 2, -1, 15);
		itemqueue.Add("jumpboost");
	}
	private void JumpBoostbyEffect()
	{
		Item_add("jumpboost", 2, -1, 15);
	}

	public void _on_normal_soundscape_finished()
	{
		play_background();
	}

	private void play_background()
	{
		switch (soundscapes)
		{
			case "normal":
				NormalSoundscape.Play();
				break;


			default:
				break;
		}
	}

	private void update_health_related_effects()
	{
		if (GetNode<V2Overlay>("%overlay").in_start_menu == false)
		{
			float r1 = 0.4f;
			float g1 = 0f;
			float b1 = 0f;
			//float a = 0f;
			float rgb2 = 0.144f;
			float rgb3 = 0.2f;
			Color newcolor = new Color(rgb2, rgb2, rgb2);
			if (_healthbar.Value < 30)
			{
				newcolor = new Color(r1, g1, b1);
				low_health_particles.Emitting = true;
			}
			else
			{
				newcolor = new Color(rgb2, rgb2, rgb2);
				low_health_particles.Emitting = false;
			}
			if (healing)
			{
				newcolor = new Color(rgb3, rgb3, rgb3);
				healing_particles.Emitting = true;
			}
			else
			{
				healing_particles.Emitting = false;
			}

			if (escape_menu_active)
			{
				newcolor = new Color(0.00f, 0.00f, 0.00f);
			}

			if (_healthbar.Value == 0)
			{
				newcolor = new Color(0.00f, 0.00f, 0.00f);
			}

			canvmod.Color = newcolor;
		}
	}

	public void receive_damage(double damage)
	{
		_healthbar.Value -= damage;
		if (_healthbar.Value <= 0)
		{
			_healthbar.Value = 0;
			player_killed();
			SetProcessInput(false);
		}

		damage_particles.Restart();

		Damage.Play();
		screenshake();
		red_effect_time = 0.15;
	}

	public void player_killed()
	{
		Losing_sfx.Play();
		canvmod.Color = new Color(0.00f, 0.00f, 0.00f);
		GetTree().Paused = true;
		GetNode<V2Overlay>("%overlay").dead = true;
	}



	//move is the audiostream
	public void _on_move_finished()
	{
		//Move.Play();
	}

	private void screenshake()
	{
		screenshake_duration = 0.25;
		screenshake_strenght_dynamic = 50;
	}

	public override void take_damage(double damage)
	{
		receive_damage(damage);
	}

	
	private void DownDashImpact()
	{
		DownDashImpactSFX.Play();
		Print("DownDashImpact");
		screenshake();
		for (int i = 0; i < downdash_area.Count; i++)
		{
			Print("damage to:" + downdash_area[i]);
			downdash_area[i].take_damage(34);
		}

	}


	public void _on_dashdown_damage_area_body_entered(Node2D victim)
	{
		if (victim as GenericCharacterClass != null && victim != this)
		{
			downdash_area.Add(victim as GenericCharacterClass);
		}
	}

	public void _on_dashdown_damage_area_body_exited(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
		{
			downdash_area.Remove(victim as GenericCharacterClass);
		}
	}
	

	private void LaserschwertHit()
	{
		DownDashImpactSFX.Play();
		Print("LaserschwertHit");
		screenshake();
		for (int i = 0; i < laserschwert_area.Count; i++)
		{
			Print("damage to:" + laserschwert_area[i]);
			laserschwert_area[i].take_damage(34);
		}

	}


	public void _on_laserschwert_area_body_entered(Node2D victim)
	{
		if (victim as GenericCharacterClass != null && victim != this)
		{
			laserschwert_area.Add(victim as GenericCharacterClass);
		}
	}

	public void _on_laserschwert_area_body_exited(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
		{
			laserschwert_area.Remove(victim as GenericCharacterClass);
		}
	}


	public void Bone_picked_up()
	{
		Print("mjam lecker");
	}

	public void SetEscMenuModulate(bool active)
	{
		escape_menu_active = active;
	}


}
