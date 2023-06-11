using Godot;
using System;

public partial class Respawn : Node2D
{ 
    RomaGay Roma;
	public override void _Ready(){
		Roma = GetNode<RomaGay>("/root/RomaGay");
	}
	public void RespawnW(int damage){
        
	}
	public void falled(Node2D body){
		if (Roma.lives > 0 ){
            Roma.LoseHeart(1);
		    GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D").Position=GetNode<Test1>("/root/Test1").RespawnZone;
		}
	}

	
}
