using Godot;
using System;

public partial class Projectile : RigidBody2D
{	
	private Sprite2D sprite;
	private bool projectileActive;
	private Node2D gun;
	private Vector2 newPortalPos;
	private Node2D portalController;
	private Area2D area;
	private CharacterBody2D player;
	private float speed = 400.0f;
	private VisibleOnScreenNotifier2D notifier;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred(nameof(AssignReferences));
		notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
    	notifier.ScreenExited += OnScreenExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Shoot")){
			Shoot(gun.GlobalPosition);
		}
	}
	
	public void Shoot(Vector2 playerPos){
		if (projectileActive == false) {
		this.GlobalPosition = new Vector2(playerPos.X, playerPos.Y);
		sprite.Visible = true;
		sprite.Rotation = gun.Rotation;
		RotateProjectileToMouse();
		Vector2 direction = Vector2.Right.Rotated(Rotation); 
		LinearVelocity = direction * speed;
		projectileActive = true;}
	}
	
	public void EndShot(){
		sprite.Visible = false;
		projectileActive = false;
		
	}

	public void OnAreaBodyEntered(Node2D body){
		if (body is RigidBody2D){
		EndShot();}

	}

	public void OnScreenExited(){
		EndShot();
	}

	private void RotateProjectileToMouse()
	{
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 directionToMouse = mousePosition - GlobalPosition;
			float angle = directionToMouse.Angle();
			Rotation = angle;
	}





	public void AssignReferences(){
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		sprite = GetNode<Sprite2D>("/root/Game/World/Projectile/Sprite2D");
		if ( sprite == null){GD.Print("Nope");};
		gun = player.GetNode<Sprite2D>("gunPivot/gunSprite");
		if(gun == null){GD.Print("Nope");};
		portalController = GetNode<Node2D>("/root/Game/PortalController");
		area = GetNode<Area2D>("/root/Game/World/Projectile/Area2D");
		area.BodyEntered += OnAreaBodyEntered;
		sprite.Visible = false;
		projectileActive = false;
	}


}
