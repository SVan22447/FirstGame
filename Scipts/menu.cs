using Godot;
using System;
public partial class menu : BoxContainer{
    PackedScene OptionsInstance;
    Button play;
    CpuParticles2D Particle;
    PointLight2D Light;
    Control OptionsV;
    public override void _Ready(){
        Particle =GetNode<CpuParticles2D>("CPUParticles2D");
        Light =GetNode<PointLight2D>("PointLight2D");
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
    public void PlayParticle(){
        play.GrabFocus();
        Particle.Visible = true;
        Light.Visible = true;
        Light.Position=new Vector2(136.847f,15.335f);
        Particle.Position=new Vector2(-27.657f,15.335f);
    }
    public void OptionsParticle(){
        GetNode<Button>("VBoxContainer/Button3").GrabFocus();
        Particle.Visible = true;
        Light.Visible = true;
        Light.Position=new Vector2(136.847f,51.509f);
        Particle.Position=new Vector2(-27.657f,51.509f);
    }
}

