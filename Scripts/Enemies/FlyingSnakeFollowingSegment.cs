using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class FlyingSnakeFollowingSegment : FlyingSnakeSegment
{
	[Export] FlyingSnakeSegment LeadingSegment;



	//int DistanceBetweeen = 35;



	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		/* if (!(LeadingSegment.GlobalPosition - GlobalPosition == DistanceBetween))
		{
			GlobalPosition = LeadingSegment.GlobalPosition - DistanceBetween;
		} */
		Vector2 DirectionToLeader = LeadingSegment.GlobalPosition - GlobalPosition;
		if (DirectionToLeader.Length() > 50)
		{
			DirectionToLeader = DirectionToLeader.Normalized();
			Velocity = DirectionToLeader * 200f;
			MoveAndSlide();
		}
		else
		{
			if (DirectionToLeader.Length() > 30)
			{
				DirectionToLeader = DirectionToLeader.Normalized();
				Velocity = DirectionToLeader * 0f;
				MoveAndSlide();
			}
			else
			{
				DirectionToLeader = DirectionToLeader.Normalized();
				Velocity = DirectionToLeader * 0f;
				MoveAndSlide();
			}
			
		}
		
	}

	//disabled to make damage independent from chain lenght
	/* public override void take_damage(double damage)
	{
		LeadingSegment.take_damage(damage);
	} */
}
