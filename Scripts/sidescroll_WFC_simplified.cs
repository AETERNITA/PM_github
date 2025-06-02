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

    //RoomData
    private List<string> RoomName = new List<string>();
    private Dictionary<string, string> RoomRefDict = new Dictionary<string, string>();
    private Dictionary<string, bool> OpenTopDict = new Dictionary<string, bool>();
    private Dictionary<string, bool> OpenBottomDict = new Dictionary<string, bool>();
    private Dictionary<string, bool> OpenRightSideDict = new Dictionary<string, bool>();
    private Dictionary<string, bool> OpenLeftSideDict = new Dictionary<string, bool>();


	public override void _Ready()
    {
        if (start_height > level_height)
        {
            PrintErr("start_height larger than level_height");
        }
        else
        {
            //gen_initialization();
            // new_gen_cycle();
        }
    }

 
    private void place_scene(string scene_uid, Vector2 position)
    {
        var scene = GD.Load<PackedScene>(scene_uid);
        var instance = scene.Instantiate();
        Node2D instance2D = instance as Node2D;
        if (instance2D != null)
        {
            AddChild(instance2D);
            //await ToSignal(GetTree(), "process_frame"); // wartet einen Frame ab
            instance2D.Position = position;
        }  
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
        string scenenene = "uid://bm42aiylfo3oc";
        
        place_scene(scenenene, new Vector2(5000, 0));
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
        RoomName.Add("down");
        RoomRefDict.Add("down", "uid://dofa4avgjwor3");
    }

}
