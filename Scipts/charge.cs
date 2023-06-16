using Godot;
using System;

public partial class charge : Node2D{
    CharacterBody2D GG;
	RomaGay Roma;
    UI ui;
    [Export(PropertyHint.Range,"0.01,2")]double buffVal;
    public override void _Ready(){
        ui=GetNode<UI>("/root/Test1/UI");
        GG=GetNode<CharacterBody2D>("/root/Test1/CharacterBody2D");
		Roma = GetNode<RomaGay>("/root/RomaGay");
    }
    public void ChargeBullet(Node2D body){
        GG.bulletAmount=4;
        ui.RechargeView();
		GG.AddJumping= true;
		Roma.Hilling(1);
	}
    public void BustCharge1(Node2D body){
        GG.buff = buffVal;
        GG.RechargeCir.MaxValue = buffVal;
        GG.RechargeCir.Step = buffVal/55;
	}
    public void DefCharge1(Node2D body){
        GG.buff = 1;
        GG.RechargeCir.MaxValue = GG.ShootCooldown;
        GG.RechargeCir.Step = GG.ShootCooldown/55;
    }
}
