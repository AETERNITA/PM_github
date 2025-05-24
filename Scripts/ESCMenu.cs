using Godot;
using System;
using System.Collections.Generic;

public partial class ESCMenu : Control
{
    private List<Container> categ;

    public override void _Ready(){
        categ = new List<Container>();
        foreach (Container d in this.GetChildren()){
            categ.Add(d);
        }
        foreach (Container c in categ){
            //c.Visible = false;
        }
        GD.Print("HI");
        this.ZIndex = -10;
    }

    public void CallMainESC(){
        foreach (Container c in categ){
            if(c.Name == "MainESC"){
                c.Visible = true;
                this.ZIndex = 2;
            }
        }
    }
    public void Resume(){
         foreach (Container c in categ){
            c.Visible = false;
         }
    }
    public void ReturnToMain(){
        //Noch einfügen (Main Menu fehlt aktuell)
    }

    public override void _Process(double delta){
        if(Input.IsActionJustPressed("escape")){
            CallMainESC();
            GD.Print("Debug");
        }
    }
}
