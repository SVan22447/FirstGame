using Godot;
using System;

public partial class RomaGay : Node{
	public int MaxLives = 5;
	public int lives;
	public bool isFullscreen;
	public UI Ui;
	[Signal]public delegate void CollideCheckpointEventHandler(Vector2 CheckpointPos);
	public override void _Ready(){
	    lives=MaxLives;
		ProcessMode = ProcessModeEnum.Always;
		if(DisplayServer.WindowGetMode()==DisplayServer.WindowMode.Fullscreen){
            isFullscreen=true;
		}else{
			isFullscreen=false;
		}
	}
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("FullScreenButton")&&(isFullscreen==true)){
			isFullscreen=false;
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}else if(Input.IsActionJustPressed("FullScreenButton")&&(isFullscreen==false)){
			isFullscreen=true;
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
    }
	public void LoseHeart(int damage){
		lives-=damage;
		Ui.LoadHearts();
		if (lives <=0){	
		    GetTree().Paused = true;
            GetNode<Control>("/root/Test1/UI/Control").Visible = false;
		    GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D").Visible = false;
			GetNode<Control>("/root/Test1/UI/GameOverScreen").Visible = true;
		}
		GetNode<SoundPlayer>("/root/SoundPlayer").HurtPlayedPlayer();
	}
	public void Hilling(int hill){
		if (lives < MaxLives){
			lives +=hill;
	    }
		Ui.LoadHearts();
	}
}
