using Godot;
using System;

public partial class Background2 : Node2D
{
    public override void _Ready()
    {
        GlobalPosition = GetNode<Camera2D>("../Camera2D2").GetScreenCenterPosition();
    }

    public override void _Process(double delta)
    {
        GlobalPosition = GetNode<Camera2D>("../Camera2D2").GetScreenCenterPosition();
    }
}
