using Godot;
using System;
using System.CodeDom.Compiler;
using System.Xml.Resolvers;

public partial class bullet : CharacterBody2D{
	int speed = 28000;
	float gravity = 1000;
	Timer BulletTime;
	CpuParticles2D particle;
	Timer ParticleTime; 
	Vector2 velocity;
	Vector2 f;
	public override void _Ready(){
		particle=GetNode<CpuParticles2D>("CPUParticles2D");
		BulletTime=GetNode<Timer>("BulletLife");
		ParticleTime=GetNode<Timer>("ParticleLife");
	    f = GetGlobalMousePosition()-this.Position;
	}
	public override void _Process(double delta){
		particle.Set("direction",-f);
		if (!BulletTime.IsStopped()){
			var deltaf = (float)delta;
			velocity = Velocity;
			velocity=f.Normalized()*deltaf*speed;
			velocity.Y +=  gravity * (float)delta;
			for (int i = 0; i < GetSlideCollisionCount(); i++){
				BulletStop();
				BulletTime.Stop();
			}
			Velocity = velocity;
			MoveAndSlide();
		}
	}
	private void BulletStop(){
		particle.Emitting=false;
		GetNode<Sprite2D>("Sprite2D").Visible=false;
		ParticleTime.Start();
	}
	private void ParticleStop(){
		QueueFree();
	}
	public void Boom(){
	}
}
