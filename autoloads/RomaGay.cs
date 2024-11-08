using Godot;
using Godot.NativeInterop;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
public partial class RomaGay : Node{
	public int MaxLives = 5;
	public int lives;
	public bool isFullscreen;
	public DisplayServer.WindowMode Lastscreen;
	public UI Ui;
	public Vector2 SaveCheckpoint;
	public PlayerStats StatsN;
	public PlayerStats statsG {
		get{return StatsN;}
		set{
			StatsN=value;
			SetStats(StatsN);}
	}
	[Signal]public delegate void CollideCheckpointEventHandler(Vector2 CheckpointPos);

	public override void _Ready(){
		StatsN = new PlayerStats();
		ProcessMode = ProcessModeEnum.Always;
		if(DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen())==DisplayServer.WindowMode.Fullscreen){
            isFullscreen=true;
		}else{
			isFullscreen=false;
		}
		statsG.Lives=MaxLives;
	}
	public void SetStats(PlayerStats newStats){
		statsG = newStats;
		SetPhysicsProcess(statsG != null);
	}
    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("FullScreenButton")&&DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen())==DisplayServer.WindowMode.Fullscreen){
			isFullscreen=false;
			GD.Print(Lastscreen);
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}else if(Input.IsActionJustPressed("FullScreenButton")&&DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen())!=DisplayServer.WindowMode.Fullscreen){
			Lastscreen=DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen());
			isFullscreen=true;
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			GD.Print(Lastscreen);
		}
    }
	public void LoseHeart(int damage){
		lives-=damage;
		statsG.Lives=lives;	
		Ui.LoadHearts();
		if (lives <=0){	
		    GetTree().Paused = true;
			GetNode<RomaGay>("/root/RomaGay").MaxLives=5;
			var heart = Ui.GetNode<TextureRect>("Control/HeartUp");
        	heart.Size = new Vector2(GetNode<RomaGay>("/root/RomaGay").lives*27, heart.Size.Y);		
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
		statsG.Lives=lives;	
		GetNode<Save>("/root/Save").Load();
		Ui.LoadHearts();
	}
}
