using Godot;
using Godot.Collections;
using System;
using System.Reflection;

public enum PlayerState{
	Standing,
	Walking,
	Climping,
	Hurting,
	Shooting,
	Peak,
	descent
}
public partial class Player : CharacterBody2D{
	#region переменные
		PlayerState currentState = PlayerState.Standing;
		[Export]public int Speed = 125;
		// [Export]PackedScene BallonX;
		[ExportSubgroup("Прыжок")]
		[Export(PropertyHint.Range,"0f,1f")]float jumpTimeToPeak = .2f;
		[Export(PropertyHint.Range,"0f,1f")]float jumpTimeToDescent =.3f;
		[Export(PropertyHint.Range,"0.001,1")]double coyoteTime = 0.106;
		Timer LastOnGroundTime;
		Timer FallingTimes;
		Timer DamagesTimes;
		Timer LastJump;
		Timer wallJumpLeftTime;
		Timer wallJumpRightTime;
		Timer InvincibleTimer;
		Timer KnockBackEffectTime;
		Timer KnockBackStop;
		Timer TimeForEffect;
		double SlidingOffTheWall;
		double CircleVal;
		float jumpGravity;
		float jumpVelocity;
		float fallGravity;
		bool IsJumping;
		bool IsJumping2;
		bool DamageHasTaken;
		int TimeJump;
		int TimeJump2;
		RayCast2D LeftWall;
		RayCast2D RightWall;
		bool shootBool;
		[Export]int jumpheight = 25;
		[ExportSubgroup("Выстрел")]
		[Export]double recoilX= 1700;
		[Export]double recoilY=215;
		[Export(PropertyHint.Range,"0.001,1")] public double BowCooldown = 0.3;
		[Export(PropertyHint.Range,"0.01,3")] public double ShootCooldown = 1;
		Resource DialogueResource;
		string dialogue = "start";
		public bool MovingPlatformColl;
		public bool ShootCooldownS = false;
		public double BowCooldownV;
		public double ShootCooldownV;
		public double Bowbuff=1;
		public double buff = 1;
		Vector2 MouseDirection;
		Vector2 direction;
		Node2D Bow;
		public TextureProgressBar RechargeCir;
		PackedScene bulletInstance;
		PackedScene KnockBackScene;
		public bool AddJumping;
		public short bulletAmount;
		public Sprite2D sprite;
		UI ui;
		Vector2 velocity;
		Vector2 TestVelocity;
		float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	#endregion
	float GetGravity(){
		return velocity.Y < .0f ? jumpGravity : fallGravity;
	}
	public override void _Ready(){
		// DialogueResource = ResourceLoader.Load("res://dialogue/dia.dialogue");
		LastOnGroundTime = GetNode<Timer>("Timers/LastOnGroundTime");
		LastOnGroundTime.WaitTime=coyoteTime;
		FallingTimes = GetNode<Timer>("Timers/FallingTimes");
		wallJumpLeftTime= GetNode<Timer>("Timers/wallJumpLeftTime");
		wallJumpLeftTime.WaitTime=coyoteTime;
		wallJumpRightTime= GetNode<Timer>("Timers/wallJumpRightTime");
		wallJumpRightTime.WaitTime=coyoteTime;
		LastJump = GetNode<Timer>("Timers/LastJump");
		DamagesTimes = GetNode<Timer>("Timers/DamagesTimes");
		TimeForEffect = GetNode<Timer>("Timers/TimeForEffect");
		InvincibleTimer=GetNode<Timer>("Timers/InvincibleTimer");
		KnockBackEffectTime=GetNode<Timer>("Timers/KnockbackEffectTime");
		KnockBackStop=GetNode<Timer>("Timers/KnockbackStop");
		ShootCooldownV =  ShootCooldown;
		jumpVelocity = ((2.0f*jumpheight)/jumpTimeToPeak)*-1.0f;   // подстройка значений прыжка
		jumpGravity = ((-2.0f*jumpheight)/(jumpTimeToPeak*jumpTimeToPeak))*-1.0f;
		fallGravity = ((-2.0f*jumpheight)/(jumpTimeToDescent*jumpTimeToPeak))*-1.0f;
		LeftWall = GetNode<RayCast2D>("Left");
		RightWall = GetNode<RayCast2D>("Right");
	    sprite = GetNode<Sprite2D>("Sprite2D"); 
	    RechargeCir=GetNode<TextureProgressBar>("TextureProgressBar");
	    RechargeCir.MaxValue = ShootCooldown;
	    Bow = GetNode<Node2D>("Bow");
		ui=GetNode<UI>($"/root/{GetTree().CurrentScene.Name}/UI");
		KnockBackScene =GD.Load<PackedScene>("res://Scenes/Effects/KnockBackEffect.tscn");
	    bulletInstance = GD.Load<PackedScene>("res://Scenes/Objects/Bullet.tscn");
	}
	public override void _Process(double delta){
		velocity=Velocity;
		#region Таймеры
			if (BowCooldownV>=0){ // перезарядка выстрела
                if (BowCooldownV==BowCooldown*Bowbuff){
                    CircleVal = 0;
					RechargeCir.MaxValue = BowCooldown*Bowbuff;
					RechargeCir.Visible = true;
				}
				BowCooldownV-=delta;
				CircleVal +=delta*buff;
				RechargeCir.Value = CircleVal;
				if(BowCooldownV<=0){
					RechargeCir.Visible = false;
				}
			}
			if (ShootCooldownV >= 0&&bulletAmount<4){  // Перезарядка стрел
				if(ShootCooldownV == ShootCooldown){
						ShootCooldownS=true;
				}
				ShootCooldownV -=delta*buff;
				if (ShootCooldownV<=0){
		            ShootCooldownS=false;	
					if(bulletAmount<=3){
						ShootCooldownV=ShootCooldown;
						bulletAmount+=1;				
					}
					ui.RechargeView();
				}
			}
			if(IsJumping2){
				TimeJump2--;
				if(TimeJump2<=0){
					IsJumping2 = false;
				}
			}
			if (IsJumping){
				TimeJump--;
				if(TimeJump<=0){
					IsJumping = false;
				}
			}
		#endregion		
		var MouseDirection = -(GetGlobalMousePosition() - GlobalPosition).Normalized();
		Bow.Rotation = MouseDirection.Angle(); // направления лука и то как лук повернут
		if (Bow.Scale.Y == 1 && MouseDirection.X<0){
			sprite.FlipH = true;
			Bow.ShowBehindParent = true;
			Bow.Scale = new Vector2(Bow.Scale.X, -1);
		}else if(Bow.Scale.Y == -1 && MouseDirection.X>0){
			sprite.FlipH = false;
			Bow.ShowBehindParent = false;
			Bow.Scale = new Vector2(Bow.Scale.X, 1);
		}
		#region Прыжки движения и диалоги
			if (Input.IsActionJustPressed("Jump")){ //Джамп баффер
				LastJump.Start();
			}
			direction.X = Input.GetAxis("Left", "Right");
			direction.Y = Input.GetAxis("Up", "Down");
			if(LeftWall.IsColliding()){
				wallJumpLeftTime.Start();
			}else if(RightWall.IsColliding()){
				wallJumpRightTime.Start();
			}
			if (!IsOnFloor()){
				if(velocity.Y<=450){
					if(!IsJumping&&DamagesTimes.IsStopped()&&((LeftWall.IsColliding()&&Input.IsActionPressed("Left"))||(RightWall.IsColliding()&&Input.IsActionPressed("Right")))){
						velocity.Y= 50;
						SlidingOffTheWall -=delta;
						currentState = PlayerState.Climping;
					}
					if(FallingTimes.IsStopped()&&velocity.Y<=300){
						if(GetGravity()== fallGravity){
							TimeForEffect.Stop();
						}				
						velocity.Y+= GetGravity()*(float)delta;	
					}					
				}
		}else{
			KnockBackEffectTime.Stop();
			KnockBackStop.Stop();
			LastOnGroundTime.Start();
		}
		if(!LastJump.IsStopped()&&(!LastOnGroundTime.IsStopped()||AddJumping)){// прыжок
				AddJumping = false;
				TimeForEffect.Start();
				LastOnGroundTime.Stop();
				IsJumping = true;
				SlidingOffTheWall =0.2;
				IsJumping2 = true;
				TimeJump =40;
				TimeJump2 =17;
				velocity.Y = jumpVelocity;		
		}
		if(!IsJumping2&&!LastJump.IsStopped()){ // прыжок от стены
			if ( Input.IsActionPressed("Right") && !wallJumpLeftTime.IsStopped()){
				TimeForEffect.Start();
				velocity.X-=jumpVelocity/2f;
				SlidingOffTheWall =0.22;
				velocity.Y=jumpVelocity;
			}else if (Input.IsActionPressed("Left") && !wallJumpRightTime.IsStopped()) {
				TimeForEffect.Start();
				velocity.X+=jumpVelocity/2f;
				SlidingOffTheWall =0.22;
				velocity.Y=jumpVelocity;	    
			}
		}
		if (direction.X != 0 && !shootBool&& DamagesTimes.IsStopped()){    //движение ,работает через направление умноженную на скорость и по тихоньку ускоряется или замедляется
			velocity.X = Mathf.Lerp(velocity.X,direction.X*Speed,0.5f);
		}else if(!shootBool&& DamagesTimes.IsStopped()){ // если нет выстрела и движение кончилось ,то мы замедляемся
			velocity.X = Mathf.Lerp(velocity.X,0,0.3f);
		}
		#endregion
		if (Input.IsActionJustPressed("Shoot")&&(bulletAmount>0&&BowCooldownV<=0)){   // выстрел персонажа
			shoot(delta);
		}else{
			shootBool = false;
		}
		Velocity=velocity;
		MoveAndSlide();
	}
	private void shoot(double delta){
		if (bulletAmount>0){
			bulletAmount--;
		}
		var MouseDir = GlobalPosition.DirectionTo(GetGlobalMousePosition());
		if(!TimeForEffect.IsStopped()&&((MouseDir.X>=-.7&&MouseDir.Y>=.7)||(MouseDir.X<=.7&&MouseDir.Y>=.7))){
			KnockBackEffectTime.Start();
			KnockBackStop.Start();
		}
		BowCooldownV= BowCooldown*Bowbuff;
		ShootCooldownV = ShootCooldown;
		ui.RechargeView();
		shootBool=true;
		LastOnGroundTime.Stop();
		wallJumpLeftTime.Stop();
		wallJumpRightTime.Stop();
		var bullet = (CharacterBody2D)bulletInstance.Instantiate();
		bullet.GlobalPosition = GetNode<Node2D>("Bow/Node2D").GlobalPosition;
		bullet.RotationDegrees= GetNode<Node2D>("Bow/Node2D").GlobalRotationDegrees;
		if(Bow.ShowBehindParent == true){
			bullet.ShowBehindParent = true;
		}
		GetNode<SoundPlayer>("/root/SoundPlayer").PlaySound();
		GetTree().Root.GetNode<Node2D>($"{GetTree().CurrentScene.Name}/CameraProxy").AddSibling(bullet);
		if(velocity.Y <=.0f){
			velocity.X -= (MouseDir.X * (float)recoilX);
			velocity.Y -= (MouseDir.Y * ((float)recoilY/1.25f)); 
		}else if(velocity.Y>.0f&&MouseDir.Y>.0f){
			velocity.X = -(MouseDir.X * (float)recoilX);
			velocity.Y = -(MouseDir.Y * (float)recoilY); 
		}else if(velocity.Y>.0f&&MouseDir.Y<=.0f){
			velocity.X = -(MouseDir.X * (float)recoilX);
			velocity.Y -= (MouseDir.Y * (float)recoilY); 
		}
	}
	private void recoilSpam(){
		InstanceRecoil();
	}
	private void recoilStop(){
		KnockBackEffectTime.Stop();
	}
	private void InvincibleTimesEnd(){
		DamageHasTaken=false;
	}
	private void DamageTaken(Rid bodyRid, Node2D body,int bodyIndex,int localIndex){
		if(!DamageHasTaken){
			DamageHasTaken=true;			
			DamagesTimes.Start();
			InvincibleTimer.Start();
			TimeForEffect.Start();
			if(body is TileMap){
				var Body = (TileMap)body;
				var coords = Body.GetCoordsForBodyRid(bodyRid);
				var SpikesPos= -GlobalPosition.DirectionTo(Body.MapToLocal(coords));
				velocity= (SpikesPos * 400);
			}else{
				var EnemyDir=GlobalPosition.DirectionTo(body.GlobalPosition);
				if(body is CharacterBody2D){
					var Body = (CharacterBody2D)body;
					velocity= (EnemyDir*Body.Velocity*2.4f);
				}
			}
			Velocity=velocity;		
			GetNode<RomaGay>("/root/RomaGay").LoseHeart(1);
		}
	}
	private void InstanceRecoil(){
    	var recoil =KnockBackScene.Instantiate() as Sprite2D;
		var SpriteN= GetNode<Sprite2D>("Sprite2D");
		GetTree().Root.GetNode<Node2D>($"{GetTree().CurrentScene.Name}/CameraProxy").AddSibling(recoil);
		recoil.GlobalPosition=GlobalPosition;
		recoil.Texture=SpriteN.Texture;
		recoil.Vframes=SpriteN.Vframes;
		recoil.Hframes=SpriteN.Hframes;
		recoil.Frame=SpriteN.Frame;
		recoil.FlipH=SpriteN.FlipH;
	}
}