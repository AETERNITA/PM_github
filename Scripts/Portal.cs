using Godot;
using System;

public partial class Portal : RigidBody2D
{
private double x = 0;
private double y = 0; 
private double radiants;
private int portalType = 0;
private Vector2 portalVelocity;
private CharacterBody2D player;
private Node2D controller;
private bool teleportedTo;

	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -400);
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		teleportedTo = false;
	}

	public override void _Process(double delta)
	{
		//set_contact_monitor(true);
	}

	public int GetPortalType(){
		return this.portalType;
	}
	public void set_portal_type(int Type){
		portalType = Type;
	}
	public void set_portal_teleported_to (bool telto){
		teleportedTo = true;
	}
	public bool GetTelTo(){
		return teleportedTo;
	}
	
}
