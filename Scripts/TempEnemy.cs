using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class TempEnemy : GenericCharacterClass
{
	private List<GenericCharacterClass> victims = new List<GenericCharacterClass>();

	private double health = 100;
	private AudioStreamPlayer DeathSFX;
	private AudioStreamPlayer DamageSFX;
	private double red_time;

	public override void _Ready()
	{
		GetEnemyAudioNodes();
	}

	public virtual void GetEnemyAudioNodes()
	{
		DeathSFX = GetNode<AudioStreamPlayer>("%standart_enemy_death_sfx");
		DamageSFX = GetNode<AudioStreamPlayer>("%standart_enemy_damage_sfx");
	}

	public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < victims.Count; i++)
		{
			victims[i].take_damage(1);
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

	public override void take_damage(double damage)
	{
		Print("the box has emotions too!!");
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
		//Print("your mine" + victim);
	}

	public void _on_damage_area_body_exited(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
		{
			victims.Remove(victim as GenericCharacterClass);
		}
		//Print("miss you" + victim);
	}
	
	
}
