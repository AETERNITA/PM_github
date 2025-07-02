using Godot;
using System;

public partial class Projectilenew : Area2D
{
	private CharacterBody2D player;
	private Sprite2D sprite;
	private float direction;
	private bool projectileActive = false;
	private Node2D gun;
	private float speed = 15.0f;
	private Vector2 lastPos;
	private PortalController portalController;
	private RayCast2D ray;
	[Export] private AudioStreamPlayer shoot_sfx;
	[Export] private AudioStreamPlayer2D flying_sfx;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred(nameof(AssignReferences));
		shoot_sfx = GetNode<AudioStreamPlayer>("shooting_sfx");
		flying_sfx = GetNode<AudioStreamPlayer2D>("flying_sfx");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Shoot"))
		{
			Shoot(gun.GlobalPosition);
		}

		if (projectileActive == true)
		{
			this.GlobalPosition = new Vector2(GlobalPosition.X + Mathf.Cos(direction) * speed, GlobalPosition.Y + Mathf.Sin(direction) * speed);
		}
		
		if (Input.IsActionJustPressed("controller_shoot_portal"))
		{
			Shoot_Controller(gun.GlobalPosition);
		}
		
	}

	public void Shoot(Vector2 playerPos)
	{
		if (true)
		{
			this.GlobalPosition = new Vector2(playerPos.X, playerPos.Y);
			RotateProjectileToMouse();
			sprite.Visible = true;
			sprite.Rotation = direction;
			projectileActive = true;
			shoot_sfx.Play();
			flying_sfx.Play();
		}
	}
	public void Shoot_Controller(Vector2 playerPos){
		if (true)
		{

			this.GlobalPosition = new Vector2(playerPos.X, playerPos.Y);
			RotateProjectileToControllerDirection();
			sprite.Visible = true;
			sprite.Rotation = direction;
			projectileActive = true;
			shoot_sfx.Play();
			flying_sfx.Play();
		}
	}
	
	public void EndShot(float a)
	{
		sprite.Visible = false;
		projectileActive = false;
		lastPos = GlobalPosition;
		portalController.SpawnPortal(lastPos, a);
		flying_sfx.Stop();
	}

	public void AssignReferences(){
		portalController = GetNode<PortalController>("/root/Game/PortalController");
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		sprite = GetNode<Sprite2D>("Sprite2D");
		gun = player.GetNode<Node2D>("gunPivot/gunSprite");
		sprite.Visible = false;
		ray = GetNode<RayCast2D>("/root/Game/Interactables/RayCast2D");
	}

	private void RotateProjectileToMouse()
	{
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 directionToMouse = mousePosition - GlobalPosition;
			float angle = directionToMouse.Angle();
			direction = angle;
	}
	
	private void RotateProjectileToControllerDirection()
	{
		float x = Input.GetActionStrength("right_stick_right") - Input.GetActionRawStrength("right_stick_left");
		float y = Input.GetActionStrength("right_stick_down") - Input.GetActionRawStrength("right_stick_up");

        Vector2 Controller_direction = new Vector2(x, y);

		float angle = Controller_direction.Angle();
		direction = angle;
	}
	
	public void _on_body_entered(Node2D body)
	{
		if (body is StaticBody2D)
		{
			float a1 = 0;
			ray.Enabled = true;
			ray.GlobalPosition = GlobalPosition - new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)) * 150;
			ray.TargetPosition = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)) * 1000;
			ray.ForceRaycastUpdate();

			if (ray.IsColliding())
			{
				
				Vector2 normal = ray.GetCollisionNormal();
				a1 = Mathf.Atan2(normal.Y, normal.X);

			}
			
			EndShot(a1);
		
		}
	}
}
