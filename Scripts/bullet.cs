using Godot;
using System;

public partial class bullet : CharacterBody2D{
	int speed = 45000;
	int bulletLife = 60;
	float gravity = -1000; 
	Vector2 velocity;
	Vector2 f;
	public override void _Ready(){
	    f = GetGlobalMousePosition()-this.Position;
	}
	public override void _Process(double delta){
		bulletLife--;
		if (bulletLife<=0){
			QueueFree();
		}
		var deltaf = (float)delta;
		velocity = Velocity;
		velocity=f.Normalized()*deltaf*speed;
		velocity.Y +=  gravity * (float)delta;
		for (int i = 0; i < GetSlideCollisionCount(); i++){
			QueueFree();
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	public void Boom(){
	}
}
