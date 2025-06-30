using Godot;
using System;
using System.Collections.Generic;

public partial class LaserSchwert : Area2D
{
    private Player player;
    private List<TempEnemy> inRange;
    
    public override void _Ready(){
        player = GetNode<Player>("/root/Game/Player");
        inRange = new List<TempEnemy>();
        BodyEntered += Entered;
        BodyExited += Exited;
    }

    public override void _Process(double delta){
        if(Input.IsActionJustPressed("SwordHit")){
            CheckHit();
        }

    }
    //Missing Reference to enemy (Controller) needs to get list of all onscreen enemies
   // public bool CheckHit(String Path){
   //         if(Path == Path){ return true;} return false; }

   public void CheckHit(){
    foreach(TempEnemy body in inRange){
        if(body != GetParent()){
            body.take_damage(100);
        }
    }
   }

    public void Entered(Node2D body){
        if(body is TempEnemy enemy){
        inRange.Add(enemy);
        GD.Print(inRange);}
    }

    public void Exited(Node2D body){
           if(body is TempEnemy enemy){
            inRange.Remove(enemy);
            foreach(TempEnemy b in inRange){
            GD.Print(b);}}
        
    }
}
