using Godot;
using System;

public partial class Projectile : RigidBody2D
{	
	private Sprite2D sprite;
	private bool projectileActive;
	private Node2D gun;
	private Vector2 newPortalPos;
	private Node2D portalSpawner;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite");
		CallDeferred(nameof(AssignReferences));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Shoot(Vector2 playerPos){
		this.GlobalPosition = new Vector2(playerPos.X, playerPos.Y);
		sprite.Visible = true;
		this.Rotation = gun.Rotation;

	}
	
	public void EndShot(){
		sprite.Visible = false;
		
	}

	public void AssignReferences(){
		gun = GetNode<Node2D>("/root/Game/Player/gunPivot");
		portalSpawner = GetNode<Node2D>("/root/Game/PortalSpawner");
	}


}
