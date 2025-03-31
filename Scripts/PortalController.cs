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
	public override void _Ready()
	{
		portale = new List<Portal>();
		
	}

	public override void _Process(double delta)
	{
	}

	public void SpawnPortal(Vector2 spawnpoint){
		if (GetPortalCount() > 1){
			RigidBody2D delPortal = portale[0]; 
			delPortal.QueueFree(); 
			portale.RemoveAt(0);
			//delPortal = portale[0]; 
			//delPortal.QueueFree(); 
			//portale.RemoveAt(0);
			SpawnPortal(spawnpoint);
		}
		else{
			newportal = PortalSpawner.Instantiate<Portal>();
			GetParent().CallDeferred("add_child", newportal);
			portale.Add(newportal);
			newportal.CallDeferred("set_portal_type", portale.IndexOf(newportal));
			newportal.CallDeferred("set", "global_position", spawnpoint);
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
	public void PortalTouched(int portalId){
		touchedPortal = portalId;
		Teleport();
	}
	public void Teleport(){
		if (portale[touchedPortal].GetTelTo()){}else{
		Portal partnerPortal = GetNode<Portal>(GetPartnerPortal(touchedPortal));
		RigidBody2D rigidTouchedPortal = portale[touchedPortal];
		player.GlobalPosition = partnerPortal.GlobalPosition;
		partnerPortal.set_portal_teleported_to(true);
		}
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
