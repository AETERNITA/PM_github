//Dominik von Ehrenstein

using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class Room : Node
{
    public List<bool> connections_front;
    public List<bool> connections_back;

    public Room(List<bool> front_connections, List<bool> back_connections)
    {
        connections_back = back_connections;
        connections_front = front_connections;
    }


}
