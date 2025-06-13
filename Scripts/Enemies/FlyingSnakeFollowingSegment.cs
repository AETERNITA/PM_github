using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class FlyingSnakeFollowingSegment : FlyingSnakeSegment
{
    [Export] FlyingSnakeSegment LeadingSegment;



    int DistanceBetweeen = 35;



    public override void _Ready()
    {

    }

    /* public override void _PhysicsProcess(double delta)
    {
        if (!(LeadingSegment.GlobalPosition - GlobalPosition == DistanceBetween))
        {
            GlobalPosition = LeadingSegment.GlobalPosition - DistanceBetween;
        }
    } */
}