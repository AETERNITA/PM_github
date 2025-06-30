using Godot;
using System;

public partial class Overlay : CanvasLayer
{
	private Label Master_Label;
	private int Master_Index;
	private HSlider Master_Slider;
	private Label Stopwatch;
	private Label Points;
	public bool in_start_menu = true;
	private double time = 0;
	private double Points_number = 0;
	private CanvasModulate canvmod;
	private bool isinit = false;
	private Label Item1;
	private Label Item2;

	public override void _Ready()
	{
		Master_Label = GetNode<Label>("Master_Label");

		Master_Index = AudioServer.GetBusIndex("Master");

		Master_Slider = GetNode<HSlider>("Master_Slider");

		Stopwatch = GetNode<Label>("Stopwatch");
		Stopwatch.Text = "Time: 0";

		Points = GetNode<Label>("Points");
		Points.Text = "Points: 0";

		Master_Slider.Value = 0.5;

		Item1 = GetNode<Label>("Item 1");
		Item2 = GetNode<Label>("Item 2");
	}

	private void darken()
	{
		canvmod = GetNode<CanvasModulate>("%canvmod");
		canvmod.Color = new Color(0.00f, 0.00f, 0.00f);
	}

	public override void _Process(double delta)
	{
		if (!GetTree().Paused)
		{
			time = time + delta;
			Stopwatch.Text = "Time:" + Math.Round(time, 2).ToString();
		}
		else
		{
			time = 0;
			Stopwatch.Text = "Time:" + Math.Round(time, 2).ToString();
		}

		if (isinit == false)
		{
			darken();
			isinit = true;
			GetTree().Paused = true;
		}
	}

	public void _on_master_slider_value_changed(float myFloat)
	{
		Master_Label.Text = "Master: " + myFloat.ToString();
		AudioServer.SetBusVolumeDb(Master_Index, Mathf.LinearToDb(myFloat));
	}

	public void AddPoints(int points)
	{
		Points_number = Points_number + points;
		Points.Text = "Points: " + Points_number.ToString();
	}

	public void _on_button_pressed()
	{
		GetTree().Paused = false;
		if (in_start_menu)
		{
			Button start_button = GetNode<Button>("Play_Button");
			start_button.Hide();
			in_start_menu = false;
		}
	}

	public void override_inventory(string a, string b)
	{
		Item1.Text = a;
		Item2.Text = b;
	}
}
