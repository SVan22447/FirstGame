using Godot;
using System;

public partial class RomaGay : Node{
	public int MaxLives = 5;
	public int lives;
	public UI Ui;
	[Signal]public delegate void CollideCheckpointEventHandler(Vector2 CheckpointPos);
	public override void _Ready(){
	    lives=MaxLives;
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
