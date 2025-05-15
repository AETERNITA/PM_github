using Godot;
using System;

public partial class Overlay : CanvasLayer
{
	private Label Master_Label;
	
	private int Master_Index;
	
	private HSlider Master_Slider;
    private Label Stopwatch;
    private Label Points;

    private double time = 0;
	
	public override void _Ready(){
		Master_Label = GetNode<Label>("Master_Label");
		
		Master_Index = AudioServer.GetBusIndex("Master");
		
		Master_Slider = GetNode<HSlider>("Master_Slider");

        Stopwatch = GetNode<Label>("Stopwatch");
        Stopwatch.Text = "Time: 0";

        Points = GetNode<Label>("Points");
        Points.Text = "Points: 0";
		
		Master_Slider.Value = 0.5;
	}

    public override void _Process(double delta)
    {
        time = time + delta;
        Stopwatch.Text = "Time:" + Math.Round(time,2).ToString();
    }
	
public void _on_master_slider_value_changed(float myFloat){
	
			Master_Label.Text = "Master: " + myFloat.ToString();
			AudioServer.SetBusVolumeDb(Master_Index, Mathf.LinearToDb(myFloat));
			
	
	
}
}
