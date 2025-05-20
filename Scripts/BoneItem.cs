using Godot;
using System;

public partial class BoneItem : Node
{
	public override void _Ready()
	{
		Connect("body_entered", Callable.From<Node2D>(_on_player_collision));
	}

	
	public override void _Process(double delta)
	{
	}

	public void _on_player_collision(Node2D body)
	{
		QueueFree();
		Player player = GetNode<Player>("%Player");
		player.Bone_picked_up();
	}
}
