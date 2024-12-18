using Godot;
using System;
using System.Diagnostics;

public partial class menu : BoxContainer{
    [Export] float XButton;
    [Export] float YPlayButton;
    [Export] float YOptionsButton;
    [Export] float YCreditsButton;
    [Export] float YQuitButton;
    [Export] float YContinueButton;
    PackedScene OptionsInstance;
    Button play;
    RomaGay Roma;
    CpuParticles2D Particle;
    PointLight2D Light;
    Control OptionsV;
    public override void _Ready(){
        Roma =GetNode<RomaGay>("/root/RomaGay");
        Particle =GetNode<CpuParticles2D>("CPUParticles2D");
        Light =GetNode<PointLight2D>("PointLight2D");
        GetNode<options_menu>("/root/menu/Control/OptionsMenu").Menu = this;
        OptionsV = GetNode<Control>("/root/menu/Control");
        play = GetNode<Button>("VBoxContainer/Button");
        GetNode<options_menu>("/root/menu/Control/OptionsMenu").play = play;
        play.GrabFocus();
        GetNode<Save>("/root/Save").Load();
        if(Roma.statsG.Played){
            GetNode<Button>("VBoxContainer/Button5").Disabled=false;
        } 
    }
    public override void _Input(InputEvent @event){
        if(@event.IsActionPressed("_pause_menu")&&!GetNode<Button>("VBoxContainer/Button3").Visible){
            GetNode<Button>("VBoxContainer/Button3").EmitSignal(Button.SignalName.Pressed);
        }
    }
    public void Сontinue(){
        Roma.lives=Roma.MaxLives;
         GetTree().ChangeSceneToFile(Roma.statsG.pathLevel);
    }
    public void Play(){
        Roma.lives=Roma.MaxLives;
        Roma.statsG.Played=false;
        Roma.statsG.pos=new Vector2(-364,418);
        GetNode<Save>("/root/Save").Saving();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Game.tscn");
    }
    public void Options(){
        this.Visible = !this.Visible;
        OptionsV.Visible = !OptionsV.Visible;
        if(OptionsV.Visible){
            GetNode<Button>("/root/menu/Control/OptionsMenu/Control2/Button").GrabFocus();
        }
    }
    public void Credits(){
        this.Visible = !this.Visible;
        GetNode<Control>("/root/menu/Control2").Visible = !GetNode<Control>("/root/menu/Control2").Visible;
        if(GetNode<Control>("/root/menu/Control2").Visible){
            GetNode<Button>("/root/menu/Control2/Control/Button").GrabFocus();
        }else{
            play.GrabFocus();
        }
    }
    public void QuitEx(){
        GetTree().Quit();
    }
     public void ContinueParticle(){
        GetNode<Button>("VBoxContainer/Button5").GrabFocus();
        Light.Position=new Vector2(XButton,YContinueButton);
        Particle.Position=new Vector2(-20.298f,YContinueButton);
    }
    public void PlayParticle(){
        play.GrabFocus();
        Particle.Visible = true;
        Light.Visible = true;
        Light.Position=new Vector2(XButton,YPlayButton);
        Particle.Position=new Vector2(-20.298f,YPlayButton);
    }
    private void OptionsParticle(){
        GetNode<Button>("VBoxContainer/Button3").GrabFocus();
        Light.Position=new Vector2(XButton,YOptionsButton);
        Particle.Position=new Vector2(-20.298f,YOptionsButton);
    }
    private void CreditsParticle(){
        GetNode<Button>("VBoxContainer/Button2").GrabFocus();
        Light.Position=new Vector2(XButton,YCreditsButton);
        Particle.Position=new Vector2(-20.298f,YCreditsButton);
    }
    private void QuitParticle(){
        GetNode<Button>("VBoxContainer/Button4").GrabFocus();
        Light.Position=new Vector2(XButton,YQuitButton);
        Particle.Position=new Vector2(-20.298f,YQuitButton);
    }
}

