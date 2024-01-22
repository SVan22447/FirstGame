using Godot;
using System;
public partial class RomaGay : Node{
	public int MaxLives = 5;
	public int lives;
	public bool isFullscreen;
	public DisplayServer.WindowMode Lastscreen;
	public UI Ui;
	[Signal]public delegate void CollideCheckpointEventHandler(Vector2 CheckpointPos);

	public override void _Ready(){
		ProcessMode = ProcessModeEnum.Always;
		if(DisplayServer.WindowGetMode()==DisplayServer.WindowMode.Fullscreen){
            isFullscreen=true;
		}else{
			isFullscreen=false;
		}
	    lives=MaxLives;
	}
    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("FullScreenButton")&&isFullscreen){
			isFullscreen=false;
            DisplayServer.WindowSetMode(Lastscreen);
		}else if(Input.IsActionJustPressed("FullScreenButton")&&!isFullscreen){
			isFullscreen=true;
			Lastscreen=DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen());
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
    }
	public void LoseHeart(int damage){
		lives-=damage;
		Ui.LoadHearts();
		if (lives <=0){	
		    GetTree().Paused = true;
            GetNode<Control>($"/root/{GetTree().CurrentScene.Name}/UI/Control").Visible = false;
		    GetNode<CharacterBody2D>($"/root/{GetTree().CurrentScene.Name}/Player").Visible = false;
			GetNode<Control>($"/root/{GetTree().CurrentScene.Name}/UI/GameOverScreen").Visible = true;
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
