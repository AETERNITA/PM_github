//Dominik von Ehrenstein
using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class sidescroll_WFC_simplified : Node2D
{
    //maximum amount of stacked scenes that have the lowest possible height
    static int level_height = 4;
    static int start_height = 1;
    private bool[] open_connections = new bool[level_height];

    private List<int> gen_queue = new List<int>();

	public override void _Ready()
    {
        if(start_height > level_height)
        {
            PrintErr("start_height larger than level_height");
        }
        else
        {
            gen_initialization();
            new_gen_cycle();
        }
    }


    private void place_scene ()
    {
        var scene = GD.Load<PackedScene>("uid://bm42aiylfo3oc"); //will be a variable later
        //var instance = scene.Instantiate();
        Node2D node2d_instance = (Node2D)scene.Instantiate();
        AddChild(node2d_instance);
        //node2d_instance.set_global_position(new Vector2(150, 0));
        //node2d_instance.global_position = new Vector2(150, 0);


    }


    private void setup_start_area(int start_height)
    {
        //connected_to[start_height, 0] = true;
        open_connections[start_height] = true;
    }


    private void new_gen_cycle()
    {
        for (int i = 0; i < level_height; i++)
        {
            gen_queue.Add(i);
        }
        definitive_placement();
        WFC_placement();
    }

    private void definitive_placement()
    {

    }

    private void WFC_placement()
    {
        for (int i = 0; i < gen_queue.Count; i++)
        {
            
        }
    }

    private void gen_initialization()
    {
        open_connections[0] = true;
    }

}
