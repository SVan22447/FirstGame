using Godot;
using System;

public partial class Respawn : Node2D{ 
    RomaGay Roma;
	public override void _Ready(){
		Roma = GetNode<RomaGay>("/root/RomaGay");
	}
	public void RespawnW(int damage){  
		
	}
	public void falled(Node2D body){
		if (Roma.lives > 0 ){
            Roma.LoseHeart(1);
			var SceneName =GetTree().CurrentScene.Name;
		    GetNode<CharacterBody2D>($"/root/{SceneName}/Player").Position=GetNode<Test1>($"/root/{SceneName}").RespawnZone;
		}
	}

	
}
