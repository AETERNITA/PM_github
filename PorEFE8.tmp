using Godot;
using System;

public partial class Portal : RigidBody2D
{
private double x = 0;
private double y = 0; 
private double radiants;
private Vector2 portalVelocity;
private CharacterBody2D player;

	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -400);
		player = GetNode<CharacterBody2D>("/root/Game/Player");
  		

	}

	public override void _Process(double delta)
	{
		//set_contact_monitor(true);
	}
}
