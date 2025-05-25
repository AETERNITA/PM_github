using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class TempEnemy : GenericCharacterClass
{

	[Export] public float Speed = 150f;
	[Export] public float Gravity = 800f;
    [Export] public Vector2 Direction = Vector2.Left;

	private List<GenericCharacterClass> victims = new List<GenericCharacterClass>();

	private double health = 100;
	private double red_time;

	private AudioStreamPlayer DeathSFX;
	private AudioStreamPlayer DamageSFX;
	
    private RayCast2D _wallRay;
    private RayCast2D _groundRay;

    private Sprite2D _sprite;

	public override void _Ready()
	{
		GetEnemyAudioNodes();
		_groundRay = GetNode<RayCast2D>("GroundRay");
        _sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
	}

	
	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;
		 
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
	}

	public virtual void GetEnemyAudioNodes()
	{
		DeathSFX = GetNode<AudioStreamPlayer>("%standart_enemy_death_sfx");
		DamageSFX = GetNode<AudioStreamPlayer>("%standart_enemy_damage_sfx");
	}


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
	
	public void deal_damage()
	{
		for (int i = 0; i < victims.Count; i++)
		{
			victims[i].take_damage(1);
		}
	}

	public override void take_damage(double damage)
	{
		Print("Enemy damaged");
		health = health - damage;
		if (health <= 0)
		{
			var OverlayRef = GetNode("%overlay") as V2Overlay;
			OverlayRef.AddPoints(10000);
			DeathSFX.Play();
			QueueFree();
		}
		else
		{
			var OverlayRef = GetNode("%overlay") as V2Overlay;
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
	
	
}
