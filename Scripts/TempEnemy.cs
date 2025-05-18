using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class TempEnemy : GenericCharacterClass
{
	private List<GenericCharacterClass> victims = new List<GenericCharacterClass>();

	private double health = 100;

   public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < victims.Count; i++)
		{
			victims[i].take_damage(1);
		}
	}

	public override void take_damage(double damage)
	{
		health = health - damage;
		if (health <= 0)
		{
			QueueFree();
		}
	}

	public void _on_damage_area_body_entered(Node2D victim)
	{
		if (victim as GenericCharacterClass != null)
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
