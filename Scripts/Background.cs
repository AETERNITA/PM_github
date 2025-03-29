using Godot;
using System;

public partial class Background : Sprite2D
{	
	private CharacterBody2D player;
	private Vector2 lastPlayerpos;
	private Vector2 newPos;
	// Called when the node enters the scene tree for the first time.
	 
	public override void _Ready()
	{
		player = GetNode<CharacterBody2D>("/root/Game/Player");
		lastPlayerpos = new Vector2(player.GlobalPosition.X, player.GlobalPosition.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		newPos = new Vector2(GlobalPosition.X + (player.GlobalPosition.X-lastPlayerpos.X) /10, (GlobalPosition.Y));
		GlobalPosition = newPos;
		lastPlayerpos = new Vector2(player.GlobalPosition.X, player.GlobalPosition.Y);
	}
}
