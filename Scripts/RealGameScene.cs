using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class RealGameScene : Node2D
{
    public void SpawnEnemy(Vector2 EnemyPosition, string EnemyType)
    {
        var scene = Load<PackedScene>(EnemyType);
        var instance = scene.Instantiate();
        Node2D instance2D = instance as Node2D;
        if (instance2D != null)
        {
            AddChild(instance2D);
            instance2D.Position = EnemyPosition;
        }
    }
}
