using Godot;
using System;

public partial class Jumpboost : Item
{
    public override void _on_player_collision(Node2D body)
    {
        GetNode<Player>("/root/Game/%Player")._on_jump_boosted(new Node2D());
        QueueFree();
    }

}
