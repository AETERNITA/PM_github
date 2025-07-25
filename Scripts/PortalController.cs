using Godot;
using System;
using System.Collections.Generic;

public partial class PortalController : Node2D
{
	private Portal newportal;
	[Export] public PackedScene PortalSpawner;
	List<Portal> portale;
	[Export] private CharacterBody2D player;
	private int touchedPortal;
	private AudioStreamPlayer PortalSpawn_sfx;
	private AudioStreamPlayer Teleport_sfx;
	public override void _Ready()
	{
		portale = new List<Portal>();
		Teleport_sfx = GetNode<AudioStreamPlayer>("Teleport");
		PortalSpawn_sfx = GetNode<AudioStreamPlayer>("PortalSpawn");
	}

	public override void _Process(double delta)
	{
	}

	public void SpawnPortal(Vector2 spawnpoint, float spawnAngle){
		if (GetPortalCount() > 1){
			RigidBody2D delPortal = portale[0]; 
			delPortal.QueueFree(); 
			portale.RemoveAt(0);
			//delPortal = portale[0]; 
			//delPortal.QueueFree(); 
			//portale.RemoveAt(0);
			SpawnPortal(spawnpoint, spawnAngle);
		}
		else{
			newportal = PortalSpawner.Instantiate<Portal>();
			GetParent().CallDeferred("add_child", newportal);
			portale.Add(newportal);
			foreach(Portal p in portale){p.CallDeferred("set_portal_type", portale.IndexOf(p));}
			newportal.CallDeferred("set", "global_position", spawnpoint);
			newportal.Rotation = spawnAngle;
			PortalSpawn_sfx.Play();
		}
	}

	public String GetPartnerPortal(int touchedportal){
		switch(touchedportal){
			case 0: return portale[1].GetPath();
			case 1: return portale[0].GetPath();
			case 2: return portale[3].GetPath();
			case 3: return portale[2].GetPath();
			case 4: return portale[5].GetPath();
			case 5: return portale[4].GetPath();
			default: return "Error at Portal Index";
		}
	}
	
	public int GetPortalCount(){
		return portale.Count;
	}
	public void PortalTouched(int portalId, Node2D charBody){
		touchedPortal = portalId;
		Teleport(charBody);
	}
	public void Teleport(Node2D charBody1){
		if (portale[touchedPortal].GetTelTo()){}else{
			if(charBody1 is CharacterBody2D charBody){
		Portal partnerPortal = GetNode<Portal>(GetPartnerPortal(touchedPortal));
		RigidBody2D rigidTouchedPortal = portale[touchedPortal];
		Vector2 playvec = charBody.Velocity;
		charBody.GlobalPosition = partnerPortal.GlobalPosition;
		partnerPortal.set_portal_teleported_to(true);
		Teleport_sfx.Play();
		float deltarot;
		deltarot = partnerPortal.Rotation-portale[touchedPortal].Rotation;
		charBody.Velocity = playvec.Rotated(deltarot+Mathf.Pi);
		GD.Print(player.Velocity);
		}}
	}
	
	public void _on_portal_body_exited(Node2D body){
		if(body is CharacterBody2D){
			foreach (Portal p in portale){
			p.set_portal_teleported_to(false);
		}}
	}
	public void PortalExited(Portal exitedPortal)
{
    exitedPortal.set_portal_teleported_to(false);
}
}
