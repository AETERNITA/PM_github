using Godot;
using System;
using System.Collections.Generic;

public partial class SaveGame : Resource
{
    [Export] int[] Highscores;
    [Export] int[] RunLenghts;
    [Export] int[] TimeofRun;
}
