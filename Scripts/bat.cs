using Godot;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
enum BatState{
	wait,
	chase,
	preperation,
	attack,
	Leave
}
public partial class bat : CharacterBody2D{
	AnimatedSprite2D Animation;
	BatState State = BatState.wait;
	NavigationAgent2D NavAgent;

	[Export] Node2D Player;
	bool RotateC;
	bool RotateV;

    public override void _Ready(){
		Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		RotateC=Animation.FlipH;
		RotateV=Animation.FlipV;
    }
	public override void _Process(double delta){
		Vector2 velocity = Velocity;
		switch (State){
			case BatState.wait:
				WaitingPlayers();
				break;
			case BatState.chase:
				break;
			case BatState.preperation:
				break;
			case BatState.attack:
				break;
			case BatState.Leave:
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	private void WaitingPlayers(){
		Animation.FlipH=RotateC;
		Animation.FlipV=RotateV;
		Animation.Play("new_animation");
	}
	private void Agring(Node2D body){

	}
}
