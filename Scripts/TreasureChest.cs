using Godot;
using System;

public partial class TreasureChest : Item
{
	public override void _on_player_collision(Node2D body)
	{
		GetNode<V2Overlay>("/root/Game/%overlay").AddPoints(100000);
		GetNode<Player>("/root/Game/%Player").ItemInteractAudio.Play();
		QueueFree();
	}
}
