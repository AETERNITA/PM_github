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
private PortalController controller;
private bool teleportedTo;
private GpuParticles2D spawn_particles;

	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -400);
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		teleportedTo = false;
		controller = GetNode<PortalController>("/root/Game/PortalController");
		if (controller == null) GD.Print("Con error");
		spawn_particles = GetNode<GpuParticles2D>("spawn_particles");
		spawn_particles.Restart();
		//GD.Print("spawned");
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
		teleportedTo = telto;
	}
	public bool GetTelTo(){
		return teleportedTo;
	}

	public void _on_body_exited(Node2D body){
		controller._on_portal_body_exited(body);
	}

	
}
