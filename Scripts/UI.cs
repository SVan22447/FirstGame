using Godot;
using System;

public partial class UI : CanvasLayer{
    CenterContainer Center;
    ColorRect Background;
    Control OptionsMenu;
    Control HpAndOther;
    Button PauseButtons;
    Button ResumeButton;
    RomaGay Roma;
    public override void _Ready(){
        LoadHearts();
        ResumeButton = GetNode<Button>("CenterContainer/VBoxContainer/Button");
        Center = GetNode<CenterContainer>("CenterContainer");
        Background = GetNode<ColorRect>("Background");
        OptionsMenu = GetNode<Control>("OptionsMenu");
        HpAndOther = GetNode<Control>("Control");
        Roma = GetNode<RomaGay>("/root/RomaGay");
        GetNode<options_menu>("OptionsMenu").Center = Center;
        Roma.Ui = this;
        GetNode<TextureRect>("Control/arrow").Visible = false;
    }
    public override void _Input(InputEvent @event)
    {  
        if (@event.IsActionPressed("_pause_menu")){
            Background.Visible = !Background.Visible;
            HpAndOther.Visible = !HpAndOther.Visible;
            if(OptionsMenu.Visible == true){
                if(GetNode<CenterContainer>("OptionsMenu/CenterContainer2").Visible){
                    GetNode<CenterContainer>("OptionsMenu/CenterContainer2").Visible=false;
                    GetNode<CenterContainer>("OptionsMenu/CenterContainer").Visible=true;
                }
                OptionsMenu.Visible = !OptionsMenu.Visible;
            }else{
                Center.Visible = !Center.Visible;
            }
            GetTree().Paused= !GetTree().Paused;
            if (GetTree().Paused){
                ResumeButton.GrabFocus();
            }
        }
    }
    private void Focus(){
        ResumeButton.GrabFocus();
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
    public void Exit(){
        GetTree().Quit();
    }
    public void Options(){
          Center.Visible = !Center.Visible;
          OptionsMenu.Visible = !OptionsMenu.Visible;
    }
    public void LoadHearts(){
        var heart = GetNode<TextureRect>("Control/Heart");
        heart.Size = new Vector2(GetNode<RomaGay>("/root/RomaGay").lives*27, heart.Size.Y);
    }
    public void RechargeView(){
        var GG = GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D");
        var arrow = GetNode<TextureRect>("Control/arrow");
        if (GG.bulletAmount>0){
            arrow.Visible=true;
            arrow.Size = new Vector2(GG.bulletAmount*9,arrow.Size.Y);
        }else{
            arrow.Visible=false;
        }
    }
}
