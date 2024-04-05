using Godot;
using System;
using System.Data;
using System.Runtime.CompilerServices;

public partial class CheckPointForSave : Sprite2D{
	private Node2D Pp;
    public override void _Ready(){
       Pp=GetParent().GetParent().GetNode<Node2D>("Player");
    }
    public override void _Process(double delta){
   		if(GetNode<Area2D>("Area2D").OverlapsBody(Pp)&&Input.IsActionJustPressed("Interact")){
			var MaP= GetNode<Test1>("/root/Test1");
			MaP.SaveZone=GlobalPosition;
			MaP.SavePointUpdate();
			GetNode<RomaGay>("/root/RomaGay").lives=GetNode<RomaGay>("/root/RomaGay").MaxLives;
			GetNode<RomaGay>("/root/RomaGay").Ui.LoadHearts();
		}  
    }
}

