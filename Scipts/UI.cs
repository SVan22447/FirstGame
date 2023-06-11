using Godot;
using System;

public partial class UI : CanvasLayer
{
    CenterContainer Center;
    ColorRect Background;
    Control OptionsMenu;
    Control HpAndOther;
    public override void _Ready(){
        LoadHearts();
        Center = GetNode<CenterContainer>("CenterContainer");
        Background = GetNode<ColorRect>("Background");
        OptionsMenu = GetNode<Control>("OptionsMenu");
        HpAndOther = GetNode<Control>("Control");
        GetNode<options_menu>("OptionsMenu").Center = Center;
        GetNode<RomaGay>("/root/RomaGay").Ui = this;
    }
    public override void _Input(InputEvent @event)
    {  
        // Enabling pause
        if (@event.IsActionPressed("_pause_menu")){
            Background.Visible = !Background.Visible;
            HpAndOther.Visible = !HpAndOther.Visible;
            if(OptionsMenu.Visible == true){
                  OptionsMenu.Visible = !OptionsMenu.Visible;
            }else{
                  Center.Visible = !Center.Visible;
            }
            GetTree().Paused= !GetTree().Paused;
            if (GetTree().Paused){
                Button Resume = GetNode<Button>("CenterContainer/VBoxContainer/Button");
                Resume.GrabFocus();
            }
        }
    }
     public void Resume(){
        // Exit pause button
        GetTree().Paused = !GetTree().Paused;
        Background.Visible = !Background.Visible;
        HpAndOther.Visible = !HpAndOther.Visible;
        Center.Visible = !Center.Visible;
    }
    public void exit_to(){
        // Exit button in the menu
		GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
    }
    public void Options(){
          Center.Visible = !Center.Visible;
          OptionsMenu.Visible = !OptionsMenu.Visible;
    }
    public void LoadHearts(){
        var heart = GetNode<TextureRect>("Control/Heart");
        heart.Size = new Vector2(GetNode<RomaGay>("/root/RomaGay").lives*27, heart.Size.Y);
    }
}
