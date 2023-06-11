using Godot;
using System;

public partial class menu : BoxContainer

{
    PackedScene OptionsInstance;
    Button play;
    Control OptionsV;
    public override void _Ready()
    {
        GetNode<options_menu>("/root/menu/Control/OptionsMenu").Menu = this;
        OptionsV = GetNode<Control>("/root/menu/Control");
        play = GetNode<Button>("VBoxContainer/Button");
        GetNode<options_menu>("/root/menu/Control/OptionsMenu").play = play;
        play.GrabFocus();
    }
    public void Play(){
        GetNode<RomaGay>("/root/RomaGay").lives=GetNode<RomaGay>("/root/RomaGay").MaxLives;
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }
    public void Options(){
        this.Visible = !this.Visible;
        OptionsV.Visible = !OptionsV.Visible;
    }
}

