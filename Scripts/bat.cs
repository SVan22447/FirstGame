using System.Runtime.CompilerServices;
using Godot;
enum BatState{
	wait,
	chase,
	preperation,
	attack,
	Leave
}
public partial class bat : CharacterBody2D{
	[Export] int Speed=100;
	[Export] int Dash=300;
	PackedScene DashScene;
	AnimatedSprite2D Animation;
	Area2D ChasingZone;
	Marker2D _spawn;
	Vector2 velocity;
	Vector2 DirToTarget;
	Timer PreperationTime;
	Timer AttackTime;
	Timer DashEffectTime;
	Timer DashStop;
	BatState State = BatState.wait;
	NavigationAgent2D NavAgent;
	[Export]Node2D Player;
	bool RotateC;
	bool RotateV;
    public override void _Ready(){
		DashScene =GD.Load<PackedScene>("res://Scenes/Effects/KnockBackEffect.tscn");
		_spawn =GetParent().GetNode<Marker2D>("Spawn");
		PreperationTime=GetNode<Timer>("Timers/PreperationTime");
		ChasingZone=GetParent().GetNode<Area2D>("Spawn/ChasingArea");
		AttackTime=GetNode<Timer>("Timers/AttackTime");
		DashEffectTime=GetNode<Timer>("Timers/DashEffectTime");
		DashStop=GetNode<Timer>("Timers/DashStop");
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
				velocity=Vector2.Zero;
				break;
			case BatState.attack:
				AttackStart(delta);
				break;
			case BatState.Leave:
				Leaving(delta);
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	private void DirectionCheck(){
		if(velocity.X>0){
			Animation.FlipH=true;
		}else if(velocity.X<0){
			Animation.FlipH=false;
		}
	}
	private void WaitingPlayers(){
		Animation.FlipH=RotateC;
		Animation.FlipV=RotateV;
		Animation.Play("Wait");
		velocity=Vector2.Zero;
	}
	private void Chasing(double delta){
		fly(Player.GlobalPosition);	
		if(Animation.FlipV){
			Animation.FlipV=false;
		}
		if(!ChasingZone.HasOverlappingBodies()){
			State=BatState.Leave;
		}
	}
	private void Leaving(double delta){
		if(NavAgent.DistanceToTarget() <10f){
			GlobalPosition=_spawn.GlobalPosition;
			State=BatState.wait;
		}
		fly(_spawn.GlobalPosition);
	}
	private void preperationToAttack(Node2D body){
		if(State!=BatState.attack&&	ChasingZone.HasOverlappingBodies()){
			State=BatState.preperation;
			velocity=Vector2.Zero;
			PreperationTime.Start();
			DashEffectTime.Start();
			DashStop.Start();
			DirToTarget= GlobalPosition.DirectionTo(Player.GlobalPosition);
		}
	}
	private void preperationEnd(){
		State=BatState.attack;
		AttackTime.Start();
	}
	private void fly(Vector2 _target){
		NavAgent.TargetPosition=_target;
		var NewVelocity =-GlobalPosition.DirectionTo(NavAgent.GetNextPathPosition());
		DirectionCheck();
		velocity=-NewVelocity*Speed;
		NavAgent.SetVelocityForced(velocity);
	}
	private void AttackStart(double delta){
		GetNode<Area2D>("Area2D").Monitoring=false;
		SetCollisionLayerValue(6,true);
		SetCollisionLayerValue(2,false);
		DirectionCheck();
		Animation.Play("Dash");
		velocity= DirToTarget*Dash;
	}
	private void AttackEnd(){
		DashStop.Stop();
		DashEffectTime.Stop();
		Animation.Play("Chase");
		GetNode<Area2D>("Area2D").Monitoring=true;
		SetCollisionLayerValue(6,false);
		SetCollisionLayerValue(2,true);
		velocity=Vector2.Zero;
		State= BatState.chase;
	}
	private void exit(Node2D body){
		if(GlobalPosition.Ceil()!=_spawn.GlobalPosition){
			State =BatState.Leave;
		}
	}
	private void Agring(Node2D body){
		Animation.Play("Chase");
		State = BatState.chase;
	}
	private void DashEffectSpam(){
		InstanceDash();
	}
	private void DashEffectStop(){
		DashEffectTime.Stop();
	}
	private void InstanceDash(){
    	var recoil =DashScene.Instantiate() as Sprite2D;
		var SpriteN= GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GetTree().Root.GetNode<Node2D>($"{GetTree().CurrentScene.Name}/CameraProxy").AddSibling(recoil);
		var CurrentFrame = SpriteN.Frame;
		var FrameN = SpriteN.SpriteFrames.GetFrameTexture("Chase",CurrentFrame);
		recoil.GlobalPosition=GlobalPosition;
		recoil.Texture=FrameN;
		recoil.FlipH=SpriteN.FlipH;
	}
	private void PositionReset(){
		GlobalPosition=_spawn.GlobalPosition;
		State=BatState.wait;
	}
}
