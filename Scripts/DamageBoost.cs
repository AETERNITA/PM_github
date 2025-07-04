using Godot;
using System;

public partial class DamageBoost : Item
{
    public override void _on_player_collision(Node2D body)
    {
        GetNode<V2Overlay>("/root/Game/%overlay").AddPoints(100);
        GetNode<Player>("/root/Game/%Player").ItemInteractAudio.Play();
        GetNode<Player>("/root/Game/%Player").AddDamageBoost();
        QueueFree();
    }
}
