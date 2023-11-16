using Godot;
using System;

public partial class GameOverScreen : Control{
	public void Continue(){
        this.Visible=!this.Visible;
		GetNode<RomaGay>("/root/RomaGay").lives = GetNode<RomaGay>("/root/RomaGay").MaxLives;
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}
	public void ExitMenu(){
        this.Visible=!this.Visible;
		GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Levels/menu.tscn");
	}
}
