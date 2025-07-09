using Godot;
using System;

public partial class tutorial : Control
{
	[Export] Player player;
	public override void _Process(){
	if (player.GiveX()>5500){
		spawn
	}
	}
}
