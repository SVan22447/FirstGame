using Godot;
using System;
using System.ComponentModel;
using System.Data;
using System.Runtime;
using System.Runtime.InteropServices;
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
	Marker2D _spawn;
	Vector2 velocity;
	BatState State = BatState.wait;
	NavigationAgent2D NavAgent;
	[Export]Node2D Player;
	bool RotateC;
	bool RotateV;
    public override void _Ready(){
		_spawn =GetParent().GetNode<Marker2D>("Spawn");
		Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		RotateC=Animation.FlipH;
		RotateV=Animation.FlipV;
    }
	public override void _Process(double delta){
		velocity = Velocity;
		switch (State){
			case BatState.wait:
				WaitingPlayers();
				break;
			case BatState.chase:
				Chasing(delta);
				break;
			case BatState.preperation:
				break;
			case BatState.attack:
				break;
			case BatState.Leave:
				Leaving(delta);
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	private void WaitingPlayers(){
		Animation.FlipH=RotateC;
		Animation.FlipV=RotateV;
		Animation.Play("Wait");
	}
	private void Chasing(double delta){
		fly(Player.GlobalPosition);
	}
	private void Leaving(double delta){
		if(NavAgent.DistanceToTarget() <10f){
			GlobalPosition=_spawn.GlobalPosition;
			State=BatState.wait;
		}
		fly(_spawn.GlobalPosition);
	}
	private void fly(Vector2 _target){
		if (NavAgent.IsNavigationFinished()){
			return;
		}
		NavAgent.TargetPosition=_target;
		var NewVelocity =(NavAgent.GetNextPathPosition()-_target).Normalized();
		if(NewVelocity.X<0){
			Animation.FlipH=true;
		}else if(NewVelocity.X>0){
			Animation.FlipH=false;
		}
		NewVelocity*=100;
		velocity=-NewVelocity;
	}
	private void exit(Node2D body){
		if(GlobalPosition.Ceil()!=_spawn.GlobalPosition){
			State =BatState.Leave;
		}
	}
	private void Agring(Node2D body){
		GD.Print("Свин идет");
		GD.Print(Player.GlobalPosition);
		Animation.Play("Chase");
		State = BatState.chase;

	}
}
