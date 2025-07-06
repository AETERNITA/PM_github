using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class WizardInsectExplosive : GenericCharacterClass
{
	/* 
		[Export] public float Speed = 150f;
		[Export] public float Gravity = 800f;
		[Export] public Vector2 Direction = Vector2.Left;
	 */
	private List<GenericCharacterClass> victims = new List<GenericCharacterClass>();
	private List<GenericCharacterClass> victims_Explosion = new List<GenericCharacterClass>();

	private double health = 20;
	private double red_time;
	private bool dead = false;
	private AudioStreamPlayer DeathSFX;
	private AudioStreamPlayer DamageSFX;
	private double timeuntilattack;
	static double attacktime = 3;

	private double TimeSinceFlash;
	private bool Flash = false;

	/* 	private RayCast2D _wallRay;
				private RayCast2D _groundRay; */

	private Sprite2D _sprite;

	private bool damaging = false;



	public override void _Ready()
	{
		GetEnemyAudioNodes();
		//_groundRay = GetNode<RayCast2D>("GroundRay");
		_sprite = GetNodeOrNull<Sprite2D>("Sprite2D");

		var originalMaterial = Material;
		var uniqueMaterial = (Material)originalMaterial.Duplicate();
		Material = uniqueMaterial;
		timeuntilattack = attacktime;
	}


	public override void _PhysicsProcess(double delta)
	{

		/* Vector2 velocity = Velocity;

		velocity.X = Direction.X * Speed;
		velocity.Y += Gravity * (float)delta;
		Velocity = velocity;
		MoveAndSlide();

		_groundRay.TargetPosition = Direction * 100 + Vector2.Down * 120;
		_groundRay.ForceRaycastUpdate();


		if (!_groundRay.IsColliding())
		{
			FlipDirection();

		}
 */
		//Print("red time");
		//Print(red_time);

		if (red_time > 0)
		{
			(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 1);
			red_time = red_time - delta;
			if (red_time < 0)
			{
				red_time = 0;
			}
		}
		else
		{
			(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 0);
		}

		deal_damage();

		timeuntilattack -= delta;
		if (timeuntilattack <= 0)
		{
			timeuntilattack = attacktime;
			if (Math.Abs(GetNode<Player>("/root/Game/%Player").GlobalPosition.X - this.GlobalPosition.X) < 1000 && Math.Abs(GetNode<Player>("/root/Game/%Player").GlobalPosition.Y - this.GlobalPosition.Y) < 500)
			{
				attack();
			}

		}

		if (Flash && TimeSinceFlash > 0.2)
		{
			if ((int)(Material as ShaderMaterial).GetShaderParameter("damage_shader_int") == 0)
			{
				(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 1);
			}
			else
			{
				(Material as ShaderMaterial).SetShaderParameter("damage_shader_int", 0);
			}
			TimeSinceFlash = 0;
		}
		TimeSinceFlash += delta;


	}

	public virtual void GetEnemyAudioNodes()
	{
		DeathSFX = GetNode<AudioStreamPlayer>("/root/Game/%standart_enemy_death_sfx");
		DamageSFX = GetNode<AudioStreamPlayer>("/root/Game/%standart_enemy_damage_sfx");
	}

	/* 
		private void _on_wall_hit(Node2D body)
		{
			FlipDirection();
		}


		private void FlipDirection()
		{
			if (IsOnFloor())
			{
				Direction = -Direction;
				var damageArea = GetNode<Node2D>("damage_area/CollisionShape2D");
				damageArea.Position = new Vector2(-damageArea.Position.X, damageArea.Position.Y);
				var WallBox = GetNode<Node2D>("wall_box/CollisionShape2D");
				WallBox.Position = new Vector2(-WallBox.Position.X, WallBox.Position.Y);
				if (_sprite != null)
				{
					_sprite.FlipH = !_sprite.FlipH;
				}
			}
		}
		*/

	public void deal_damage()
	{
		if (damaging)
		{
			for (int i = 0; i < victims.Count; i++)
			{
				victims[i].take_damage(1);
			}
		}

	}

	public override void take_damage(double damage)
	{
		Print("Enemy damaged");
		health = health - damage;
		if (health <= 0)
		{
			if (!dead)
			{
				dead = true;
				killed();
			}
		}
		else
		{
			var OverlayRef = GetNode("/root/Game/%overlay") as V2Overlay;
			OverlayRef.AddPoints(100);
		}
		DamageSFX.Play();
		red_time = 0.15;
	}

	public void _on_damage_area_body_entered(Node2D victim)
	{
		if (victim as GenericCharacterClass != null && victim != this)
		{
			victims.Add(victim as GenericCharacterClass);
		}
	}

	public void _on_damage_area_body_exited(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
		{
			victims.Remove(victim as GenericCharacterClass);
		}
	}

	private async void attack()
	{
		GetNode<Sprite2D>("damage_area/Sprite2D").Visible = true;
		damaging = false;
		GetNode<Node2D>("damage_area").GlobalPosition = GetNode<Player>("/root/Game/%Player").GlobalPosition;
		await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		damaging = true;
	}
	
	 private async void killed()
	{
		var OverlayRef = GetNode("/root/Game/%overlay") as V2Overlay;
		OverlayRef.AddPoints(10000);
		DeathSFX.Play();
		GetNode<Player>("/root/Game/%Player").AddDamageBoost();
		Flash = true;

		await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
		Explosion();
	}

	private async void Explosion()
	{
		GetNode<AudioStreamPlayer>("Explosion").Play();
		for (int i = 0; i < victims_Explosion.Count; i++)
		{
			victims_Explosion[i].take_damage(40);
		}
		_sprite.Visible = false;
		GetNode<Sprite2D>("Sprite2D/Sprite2D").Visible = false;
		await ToSignal(GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}

	public void _on_explosion_area_body_entered(Node2D victim)
	{
		if (victim as GenericCharacterClass != null && victim != this)
		{
			victims_Explosion.Add(victim as GenericCharacterClass);
			Print("hello"+victim);
		}
	}

	public void _on_explosion_area_body_exited(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
		{
			victims_Explosion.Remove(victim as GenericCharacterClass);
			Print("bye" + victim);
		}
	}
	
	
}
