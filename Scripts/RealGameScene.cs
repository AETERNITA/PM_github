using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Godot.GD;

public partial class RealGameScene : Node2D
{

    public double unpause_time = 0.1;
    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        /*         unpause_time = -delta;
                if (unpause_time < 0)
                {
                    unpause_time = 0;
                }
                if (unpause_time > 0)
                {
                    GetTree().Paused = false;
                } */
    }


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

    public async void reset_level()
    {
        GetNode<Player>("%Player").SaveTheGame();


        List<Node> nodes = GetAllNodes(GetTree().Root);

        foreach (Node item in nodes)
        {
            if (item != GetTree().Root && item != this)
            {
                item.QueueFree();
            }
        }

        await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);


        GetTree().ReloadCurrentScene();
    }
    
    public List<Node> GetAllNodes(Node root)
    {
        var nodeList = new List<Node>();
        TraverseNodes(root, nodeList);
        return nodeList;
    }

    private void TraverseNodes(Node node, List<Node> list)
    {
        list.Add(node);
        foreach (Node child in node.GetChildren())
        {
            TraverseNodes(child, list);
        }
    }
}
