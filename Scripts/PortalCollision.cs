using Godot;
using System;

public partial class PortalCollision : Area2D
{	
	private PortalController PortalController;
	private Portal Portal;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		Portal = GetParent<Portal>();
		PortalController = GetNode<PortalController>("/root/Game/PortalController");
		BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnBodyEntered(Node2D body){
			if(body is CharacterBody2D charBody){
				PortalController.PortalTouched(Portal.GetPortalType(), charBody);
			}
	}
		
		private void OnBodyExited(Node2D body)
{
    if (body is CharacterBody2D player)
    {
        PortalController.Call("PortalExited", this.GetParent()); 
    }
}
}
