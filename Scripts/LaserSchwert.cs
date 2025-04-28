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

    public override void _Process(double delta){
        if(Input.IsActionJustPressed("SwordHit")){
           // CheckHit();
        }


    }
    //Missing Reference to enemy (Controller) needs to get list of all onscreen enemies
   // public bool CheckHit(String Path){
   //         if(Path == Path){ return true;} return false; }

    public void Entered(Node2D body){
        //body.IsInGroup("World") ||
        if ( body == player){}else{
        inRange.Add(body.GetPath());
        GD.Print(inRange);}
    }

    public void Exited(Node2D body){
            inRange.RemoveAll(p => p == body.GetPath());
            foreach(String b in inRange){
            GD.Print(b);}
        
    }
}
