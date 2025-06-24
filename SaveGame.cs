using Godot;
using System;
using System.Collections.Generic;

public partial class SaveGame : Resource
{
    [Export] public int HighScore = 0;
    [Export] public int[] Highscores = Array.Empty<int>();
    [Export] public int[] RunLenghts = Array.Empty<int>();
    [Export] public int[] TimeofRun = Array.Empty<int>();
    [Export] public float Volume = 0.3f;
}
