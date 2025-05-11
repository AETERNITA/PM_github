//Dominik von Ehrenstein
using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class sidescroll_WF : Node2D
{
    //maximum amount of stacked scenes that have the lowest possible height
    static int level_height = 4;
    static int start_height = 1;
    //private bool[,] connected_to = new bool[level_height,level_height];
    private bool[] open_connections = new bool[level_height];

    private List<Room> rooms = new List<Room>();


	public override void _Ready()
    {
        if(start_height > level_height)
        {
            PrintErr("start_height larger than level_height");
        }
        else
        {
            //setup_gen_list();
            setup_start_area(start_height);
            new_gen_cycle();
        }
    }


    private void new_gen_cycle ()
    {
        create_lists();

        

    }

    private void setup_start_area(int start_height)
    {
        //connected_to[start_height, 0] = true;
        open_connections[start_height] = true;
    }

    private void create_lists()
    {

    }

    private void collapse_to_list()
    {

    }

    private void place_scenes()
    {

    }

/*     private void setup_gen_list()
    {
        //Add code for the properties of the associated scenes/rooms
        List<string> Rooms = new List<string>();
        Dictionary<string, List<bool>> front_connect_of_rooms = new Dictionary<string, List<bool>>();
        Dictionary<string, List<bool>> back_connect_of_rooms = new Dictionary<string, List<bool>>();
        // dict for reference to place scene

        //front_connect_of_rooms.Add()
    

    }
    private void add_room_to_list(List<bool> front_connect, List<bool> back_connect)
    {
        rooms.Add(new Room(front_connect, back_connect));  
    } */

}
