using Godot;
using System;

public partial class charge : Node2D{
    CharacterBody2D GG;
	RomaGay Roma;
    UI ui;
    [Export(PropertyHint.Range,"0.01,3")]double buffVal = 1.5;
    [Export(PropertyHint.Range,"0.01,2")] double BowbuffVal =0.8;
    public override void _Ready(){
        ui=GetNode<UI>("/root/Test1/UI");
        GG=GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D");
		Roma = GetNode<RomaGay>("/root/RomaGay");
    }
    public void ChargeBullet(Node2D body){
        GG.bulletAmount=4;
        GG.ShootCooldownV=0;
        GG.GetNode<TextureProgressBar>("TextureProgressBar").Visible = false;
        ui.RechargeView();
		GG.AddJumping= true;
		Roma.Hilling(1);
	}
    public void BustCharge1(Node2D body){
        GG.Bowbuff = BowbuffVal;
        GG.buff = buffVal;
	}
    public void DefCharge1(Node2D body){
        GG.Bowbuff = 1;
        GG.buff = 1;
    }
}
