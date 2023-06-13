using Godot;
using System;

public partial class charge : Node2D{
    CharacterBody2D GG;
	RomaGay Roma;
    public override void _Ready()
    {
        GG=GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D");
		Roma = GetNode<RomaGay>("/root/RomaGay");
    }
    public void ChargeBullet(Node2D body){
		if(body is CharacterBody2D){
            GG.bulletAmount=4;
			GG.AddJumping= true;
			Roma.Hilling(1);
		}
		GD.Print("Recharge");
	}
}
