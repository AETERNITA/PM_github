//Dominik von Ehrenstein
using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class sidescroll_WFC_simplified : Node2D
{
    //maximum amount of stacked scenes that have the lowest possible height
    static int level_height = -4;
    static int start_height = 1;
    int pre_gen_lenght = 2;

    //private bool[] open_connections = new bool[level_height];
    private int main_connection = start_height;
    private int currentcolumn = 1;

    private List<int> gen_queue = new List<int>();

    //RoomData
    //private List<string> RoomName = new List<string>();
    //private Dictionary<string, string> RoomRefDict = new Dictionary<string, string>();

    private List<List<string>> RoomsRooms = new List<List<string>>(); //List 1: leftright; List2: leftdown; List3: Leftup; List4: upright; List5: downright

    public override void _Ready()
    {
        if (start_height < level_height)
        {
            PrintErr("start_height larger than level_height");
        }
        else
        {
            gen_initialization();

            //Debug spawning
            //gridplace_debug();

            for (int i = 1; i <= pre_gen_lenght; i++)
            {
                new_gen_cycle(i);
                currentcolumn = i;
            }
        }
    }

    public override void _Process(double delta)
    {
        if (currentcolumn * 4000 - GetNode<Player>("../%Player").GlobalPosition.X <= 8000)
        {
            new_gen_cycle(currentcolumn + 1);
            currentcolumn++;
        }
    }



    private void place_scene(string scene_uid, Vector2 position)
    {
        var scene = Load<PackedScene>(scene_uid);
        var instance = scene.Instantiate();
        Node2D instance2D = instance as Node2D;
        if (instance2D != null)
        {
            AddChild(instance2D);
            instance2D.Position = position;
            instance2D.ProcessMode = ProcessModeEnum.Pausable;
        }
    }


    /*     private void setup_start_area(int start_height)
        {
            open_connections[start_height] = true;
        } */


    private void new_gen_cycle(int column)
    {
        gen_queue.Clear();
        for (int i = 0; i < level_height; i++)
        {
            gen_queue.Add(i);
        }

        precheck_placement(column);

        WFC_placement(column);
    }



    /* private void gridplace_debug()
    {
        int posreight = 0;
        for (int i = 0; i < level_height; i++)
        {
            posreight = 1;
            for (int j = 0; j < 100; j++)
            {
                posreight = posreight + 1;
                place_scene(RoomRefDict["leftright"], new Vector2(4000 * posreight, i * 4000));
            }
        }
    } */

    private void gen_initialization()
    {
        //open_connections[0] = true;
        /* RoomName.Add("down");
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
        RoomRefDict.Add("upright", "uid://ceo8tttylidv6"); */

        for (int i = 0; i < 5; i++)
        {
            RoomsRooms.Add(new List<string>());
        }

        //Add leftright rooms
        RoomsRooms[0].Add("uid://uf6nec4oiunb");

        //Add leftdown rooms
        RoomsRooms[1].Add("uid://bfcvxwf4tlv6l");

        //Add leftup rooms
        RoomsRooms[2].Add("uid://hbjnuoukj6pt");

        //Add upright rooms
        RoomsRooms[3].Add("uid://b81u4l1ge1p18");

        //Add downright rooms
        RoomsRooms[4].Add("uid://dimew2aifojp2");

        //setup_start_area(start_height);
    }

    private string GetRoom(string connections)
    {
        string uid = "";
        //return RoomRefDict[connections];
        switch (connections)
        {
            case "leftright":
                uid = FindRoom(0);
                break;

            case "leftdown":
                uid = FindRoom(1);
                break;

            case "leftup":
                uid = FindRoom(2);
                break;

            case "upright":
                uid = FindRoom(3);
                break;

            case "downright":
                uid = FindRoom(4);
                break;

            default:
                PrintErr("undefined room type");
                break;

        }
        return uid;
    }

    private string FindRoom(int type)
    {
        return RoomsRooms[type][(int)(Randi() % RoomsRooms[0].Count)];
    }


    private void precheck_placement(int column)
    {
        bool can_go_up;
        bool can_go_down;

        if (main_connection > level_height)
        {
            can_go_up = true;
        }
        else
        {
            can_go_up = false;
        }

        if (main_connection < 0)
        {
            can_go_down = true;
        }
        else
        {
            can_go_down = false;
        }

        /* Print(can_go_up);
        Print(can_go_down); */

        pathplace(can_go_up, can_go_down, column);
    }

    private void pathplace(bool canup, bool candown, int column)
    {
        if (RandRange(0, 1) > 0.5)
        {
            if (canup && candown)
            {
                if (RandRange(0, 1) > 0.5)
                {
                    place_scene(GetRoom("leftup"), new Vector2(4000 * column, 4000 * main_connection));
                    place_scene(GetRoom("downright"), new Vector2(4000 * column, 4000 * main_connection - 4000));
                    main_connection = main_connection - 1;
                }
                else
                {
                    place_scene(GetRoom("leftdown"), new Vector2(4000 * column, 4000 * main_connection));
                    place_scene(GetRoom("upright"), new Vector2(4000 * column, 4000 * main_connection + 4000));
                    main_connection = main_connection + 1;
                }
            }
            else
            {
                if (canup)
                {
                    place_scene(GetRoom("leftup"), new Vector2(4000 * column, 4000 * main_connection));
                    place_scene(GetRoom("downright"), new Vector2(4000 * column, 4000 * main_connection - 4000));
                    main_connection = main_connection - 1;
                }
                else
                {
                    if (candown)
                    {
                        place_scene(GetRoom("leftdown"), new Vector2(4000 * column, 4000 * main_connection));
                        place_scene(GetRoom("upright"), new Vector2(4000 * column, 4000 * main_connection + 4000));
                        main_connection = main_connection + 1;
                    }
                    else
                    {
                        place_scene(GetRoom("leftright"), new Vector2(4000 * column, main_connection * 4000));
                    }
                }
            }
        }
        else
        {
            place_scene(GetRoom("leftright"), new Vector2(4000 * column, main_connection * 4000));
        }

    }

    private void WFC_placement(int column)
    {
        for (int i = 0; i < gen_queue.Count; i++)
        {

        }
    }
    

}
