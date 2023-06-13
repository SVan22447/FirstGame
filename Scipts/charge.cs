using Godot;
using System;

public partial class charge : Node2D
{

    public void ChargeBullet(Node2D body){
		if(body is CharacterBody2D){
            GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D").bulletAmount=4;
		}
		GD.Print("Recharge");
	}
}
