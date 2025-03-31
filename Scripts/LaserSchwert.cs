using Godot;
using System;
using System.Collections.Generic;

public partial class LaserSchwert : Area2D
{
    private Player player;
    private List<String> inRange;
    
    public override void _Ready(){
        player = GetNode<Player>("/root/Game/Player");
        inRange = new List<String>();
        BodyEntered += Entered;
        BodyExited += Exited;
    }

    public bool CheckHit(String Path){
        foreach(String ListedPath in inRange){
            if(ListedPath == Path){ return true;}
            else{ return false;}
        }
        return false;
    }

    public void Entered(Node2D body){
        if (body.IsInGroup("World")){}else{
        inRange.Add(body.GetPath());
        GD.Print(inRange);}
    }

    public void Exited(Node2D body){
        foreach(String ListedPath in inRange){
            if(ListedPath == body.GetPath()){
                inRange.Remove(ListedPath);
                GD.Print(inRange);
            }
        }
    }
}
