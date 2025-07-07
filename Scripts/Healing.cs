using Godot;
using System;

public partial class Healing : Item
{
	public override void _on_player_collision(Node2D body)
	{
		GetNode<Player>("/root/Game/%Player")._on_healing_potioned(new Node2D());
		QueueFree();
	}

}
