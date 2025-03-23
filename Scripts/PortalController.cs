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
		SpawnPortal(new Vector2(0,0));
		
	}

	public override void _Process(double delta)
	{
	}

	public void SpawnPortal(Vector2 spawnpoint){
		if (GetPortalCount() > 5){
			RigidBody2D delPortal = portale[0]; 
			delPortal.QueueFree(); 
			portale.RemoveAt(0);
			delPortal = portale[0]; 
			delPortal.QueueFree(); 
			portale.RemoveAt(0);
			SpawnPortal(spawnpoint);
		}
		else{
			newportal = PortalSpawner.Instantiate<Portal>();
			GetParent().CallDeferred("add_child", newportal);
			portale.Add(newportal);
			newportal.SetPortalType(portale.IndexOf(newportal));}
			newportal.GlobalPosition = spawnpoint;
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
		RigidBody2D partnerPortal = GetNode<RigidBody2D>(GetPartnerPortal(touchedPortal));
		RigidBody2D rigidTouchedPortal = portale[touchedPortal];
		player.SetDeferred("GlobalPosition", new Vector2(partnerPortal.GlobalPosition.Y,partnerPortal.GlobalPosition.X));
	}
}
