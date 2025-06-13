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
            gen_initialization();
            gridplace_debug();
            //new_gen_cycle();
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
        
        place_scene(scenenene, new Vector2(4000, 0));
    }

    private void WFC_placement()
    {
        for (int i = 0; i < gen_queue.Count; i++)
        {
            
        }
    }

    private void gridplace_debug()
    {
        int posheight = 0;
        int posreight = 0;
        for (int i = 0; i < level_height; i++)
        {
            posreight = 1;
            for (int j = 0; j < 100; j++)
            {
                posreight = posreight + 1;
                place_scene(RoomRefDict["down"], new Vector2(4000 * posreight, i * 4000));
            }
        }
    }

    private void gen_initialization()
    {
        open_connections[0] = true;
        RoomName.Add("down");
        RoomRefDict.Add("down", "uid://dofa4avgjwor3");
        RoomName.Add("downright");
        RoomRefDict.Add("downright", "uid://dm0p773ff7q5r");
        RoomName.Add("left");
        RoomRefDict.Add("left", "uid://bm42aiylfo3oc");
        RoomName.Add("leftdown");
        RoomRefDict.Add("leftdown", "uid://bu1nylkaaafly");
        RoomName.Add("leftright");
        RoomRefDict.Add("leftright", "uid://cnhqxo33oqtbm");
        RoomName.Add("leftup");
        RoomRefDict.Add("leftup", "uid://bqvy0u2i1ce53");
        RoomName.Add("up");
        RoomRefDict.Add("up", "uid://d320lc6jfoic0");
        RoomName.Add("updown");
        RoomRefDict.Add("updown", "uid://bngjl18nokyvm");
        RoomName.Add("upright");
        RoomRefDict.Add("upright", "uid://ceo8tttylidv6");
    }

}
