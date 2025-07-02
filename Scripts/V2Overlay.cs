using Godot;
using System;
using System.Threading;

public partial class V2Overlay : Control
{
    [Export] Player player;
    [Export] AudioStreamPlayer UISound;
    private Label Master_Label;
    private int Master_Index;
    private HSlider Master_Slider;
    private Label Stopwatch;
    private Label Points;
    private Label HighScore;
    public bool in_start_menu = true;
    private double time = 0;
    public int Points_number = 0;
    private CanvasModulate canvmod;
    private bool isinit = false;
    private Label Item1;
    private Label Item2;
    private Button ResumeButton;
    private Button RestartButton;
    private bool escape_menu_active = false;
    public bool dead = false;
    private SaveGame savegame;
    private bool just_entered_esc_menu;
    private bool set_volume = false;

    /*    [Export] public PackedScene ESCMenu;
                   private Node esc;
                   private CanvasLayer layer; */

    public override void _Ready()
    {
        savegame = new SaveGame();

        if (GD.Load("user://savegame.tres") == null)
        {
            ResourceSaver.Save(savegame, "user://savegame.tres");
        }

        HighScore = GetNode<Label>("HighScore");
        HighScore.Text = "HighScore:" + savegame.HighScore;

        Master_Label = GetNode<Label>("Master_Label");

        Master_Index = AudioServer.GetBusIndex("Master");

        Master_Slider = GetNode<HSlider>("Master_Slider");

        Stopwatch = GetNode<Label>("Stopwatch");
        Stopwatch.Text = "Time: 0";

        Points = GetNode<Label>("Points");
        Points.Text = "Points: 0";

        RestartButton = GetNode<Button>("RestartButton");
        RestartButton.Visible = false;
        ResumeButton = GetNode<Button>("ResumeButton");
        ResumeButton.Visible = false;

        Master_Slider.Value = 0;

        Item1 = GetNode<Label>("Item 1");
        Item2 = GetNode<Label>("Item 2");
        /*         foreach(CanvasLayer l in GetChildren()){
                esc = ESCMenu.Instantiate();
                l.CallDeferred("add_child", esc); */
        // }

        Button start_button = GetNode<Button>("Play_Button");
        start_button.FocusMode = FocusModeEnum.All;
        start_button.GrabFocus();

        /* if (!(GD.Load("user://savegame.tres") == null))
        {
            _on_master_slider_value_changed(GD.Load<SaveGame>("user://savegame.tres").Volume);
        } */

        (GetNode<ColorRect>("Points/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 0);
        (GetNode<ColorRect>("HighScore/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 0);
        (GetNode<ColorRect>("Stopwatch/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 0);

    }

    private void darken()
    {
        canvmod = GetNode<CanvasModulate>("%canvmod");
        canvmod.Color = new Color(0.00f, 0.00f, 0.00f);
    }

    public override void _Process(double delta)
    {
        if (!set_volume && Master_Slider.Value == (GD.Load("user://savegame.tres") as SaveGame).Volume)
        {
            set_volume = true;
        }

        if (Item1.Text == "")
        {
            Item1.Visible = false;
        }
        else
        {
            Item1.Visible = true;
        }

        if (Item2.Text == "")
        {
            Item2.Visible = false;
        }
        else
        {
            Item2.Visible = true;
        }


        if (GD.Load("user://savegame.tres") == null)
        {

        }
        else
        {
            if (!set_volume)
            {
                Master_Slider.Value = GD.Load<SaveGame>("user://savegame.tres").Volume;
            }
            savegame = GD.Load("user://savegame.tres") as SaveGame;
            HighScore.Text = "HighScore:" + savegame.HighScore;
        }


        if (dead)
        {
            Item1.Visible = false;
            Item2.Visible = false;

            RestartButton.Visible = true;
            RestartButton.FocusMode = FocusModeEnum.All;
            RestartButton.GrabFocus();

            (GetNode<ColorRect>("Points/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 3);
            (GetNode<ColorRect>("HighScore/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 3);
            (GetNode<ColorRect>("Stopwatch/ColorRect").Material as ShaderMaterial).SetShaderParameter("Brightness", 3);
        }

        if (in_start_menu)
        {
            Master_Label.Visible = true;
            Master_Slider.Visible = true;
            time = 0;
        }
        else
        {
            Master_Label.Visible = escape_menu_active;
            Master_Slider.Visible = escape_menu_active;

            if (just_entered_esc_menu)
            {
                Master_Label.FocusMode = FocusModeEnum.All;
                Master_Label.GrabFocus();
            }
        }

        if (!GetTree().Paused)
        {
            time = time + delta;
            Stopwatch.Text = "Time:" + Math.Round(time, 2).ToString();
        }
        else
        {
            Stopwatch.Text = "Time:" + Math.Round(time, 2).ToString();
        }

        if (isinit == false)
        {
            darken();
            isinit = true;
            GetTree().Paused = true;
        }


        just_entered_esc_menu = false;

        if (Input.IsActionJustPressed("escape"))
        {
            UISound.Play();
            if (escape_menu_active)
            {
                escape_menu_active = false;
            }
            else
            {
                escape_menu_active = true;
                just_entered_esc_menu = true;
            }
            if (in_start_menu)
            {
                escape_menu_active = false;
                just_entered_esc_menu = false;
            }
        }

        if (Input.IsActionJustPressed("start_game"))
        {
            GetTree().Paused = false;
            if (in_start_menu)
            {
                Button start_button = GetNode<Button>("Play_Button");
                start_button.Hide();
                in_start_menu = false;
                UISound.Play();
                GetNode<Sprite2D>("logo").Visible = false;
            }
        }

        if (dead)
        {
            RestartButton.Visible = true;
        }
        else
        {
            RestartButton.Visible = escape_menu_active;
        }
        
        ResumeButton.Visible = escape_menu_active;

        player.SetEscMenuModulate(escape_menu_active);

        if (escape_menu_active && !in_start_menu && !dead)
        {
            GetTree().Paused = true;
        }
        if (!escape_menu_active && !in_start_menu && !dead)
        {
            GetTree().Paused = false;
        }

        if (GetTree().Paused == true)
        {
            darken();
        }

        
    }

    public void _on_master_slider_value_changed(float myFloat)
    {
        Master_Label.Text = "Master: " + myFloat.ToString();
        AudioServer.SetBusVolumeDb(Master_Index, Mathf.LinearToDb(myFloat));

        var savegame = new SaveGame();
        savegame.Volume = myFloat;
        ResourceSaver.Save(savegame, "user://savegame.tres");
        set_volume = true;
    }

    public void AddPoints(int points)
    {
        Points_number = Points_number + points;
        Points.Text = "Points: " + Points_number.ToString();
    }

    public void _on_button_pressed()
    {
        UISound.Play();
        GetTree().Paused = false;
        GetNode<Sprite2D>("logo").Visible = false;
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

    public void _on_resume_button_pressed()
    {
        UISound.Play();
        escape_menu_active = false;
    }

    public async void _on_restart_button_pressed()
    {
        UISound.Play();
        await ToSignal(GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);

        GetNode<RealGameScene>("/root/Game").reset_level();
    }
    

}
