using Godot;
using System;

public partial class Lit_energy : Node
{
	Player player;
	RomaGay roma;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		player=GetParent().GetNode<Player>("Player");
		roma =GetNode<RomaGay>("/root/RomaGay");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if(GetNode<Area2D>("Area2D").OverlapsBody(player)&&Input.IsActionJustPressed("Interact")){
			GD.Print(" смешно");
			player.Scale=new Vector2(0.5f,0.5f);
			player.recoilX*=2f;
			player.recoilY*=2f;
			roma.MaxLives=3;
			roma.lives=3;
			var heart = roma.Ui.GetNode<TextureRect>("Control/HeartUp");
        	heart.Size = new Vector2(GetNode<RomaGay>("/root/RomaGay").lives*27, heart.Size.Y);			
			roma.Ui.LoadHearts();

		}
	}
}
