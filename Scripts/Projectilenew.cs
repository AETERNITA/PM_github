using Godot;
using System;

public partial class Projectilenew : Area2D
{
	private CharacterBody2D player;
	private Sprite2D sprite;
	private float direction;
	private bool projectileActive = false;
	private Node2D gun;
	private float speed = 5.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred(nameof(AssignReferences));
	} 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{ 
		if(Input.IsActionJustPressed("Shoot")){
			Shoot(gun.GlobalPosition);
		}
	
		if(projectileActive == true){
			this.GlobalPosition = new Vector2 (GlobalPosition.X+Mathf.Cos(direction)*speed, GlobalPosition.Y + Mathf.Sin(direction)*speed);
		}
	}

	public void Shoot(Vector2 playerPos){
		if (projectileActive == false) {
		RotateProjectileToMouse();
		this.GlobalPosition = new Vector2(playerPos.X, playerPos.Y);
		sprite.Visible = true;
		sprite.Rotation = direction;
		projectileActive = true;}
	}
	
	public void EndShot(){
		sprite.Visible = false;
		projectileActive = false;
		
	}

	public void AssignReferences(){
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		sprite = GetNode<Sprite2D>("Sprite2D");
		gun = player.GetNode<Node2D>("gunPivot/gunSprite");
		sprite.Visible = false;
	}

	private void RotateProjectileToMouse()
	{
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 directionToMouse = mousePosition - GlobalPosition;
			float angle = directionToMouse.Angle();
			direction = angle;
	}
}
