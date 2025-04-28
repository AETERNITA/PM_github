using Godot;
using System;

public partial class AudioSlider : CanvasLayer
{
	private Label Master_Label;
	
	private int Master_Index;
	
	private HSlider Master_Slider;
	
	public override void _Ready(){
		Master_Label = GetNode<Label>("Master_Label");
		
		Master_Index = AudioServer.GetBusIndex("Master");
		
		Master_Slider = GetNode<HSlider>("Master_Slider");
		
		Master_Slider.Value = 0.5;
	}
	
public void _on_master_slider_value_changed(float myFloat){
	
			Master_Label.Text = "Master: " + myFloat.ToString();
			AudioServer.SetBusVolumeDb(Master_Index, Mathf.LinearToDb(myFloat));
			
	
	
}
}
