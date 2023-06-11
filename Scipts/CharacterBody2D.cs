using Godot;
using Godot.Collections;
using DialogueManagerRuntime;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	#region переменные
		[Export]public int Speed = 125;
		[Export]PackedScene BallonX;
		[ExportSubgroup("Прыжок")]
		[Export(PropertyHint.Range,"0f,1f")]float jumpTimeToPeak = .2f;
		[Export(PropertyHint.Range,"0f,1f")]float jumpTimeToDescent =.3f;
		[Export(PropertyHint.Range,"0.001,1")]double coyoteTime = 0.106;
		double LastOnGroundTime;
		double CircleVal;
		float jumpGravity;
		float jumpVelocity;
		float fallGravity;
		bool IsJumping;
		bool IsJumping2;
		int TimeJump;
		int TimeJump2;
		RayCast2D LeftWall;
		RayCast2D RightWall;
		double wallJumpLeftTime;
		double wallJumpRightTime;
		double LastJump;
		[Export]int jumpheight = 25;
		[ExportSubgroup("Выстрел")]
		[Export]double recoilX= 1700;
		[Export]double recoilY=215;
		[Export(PropertyHint.Range,"0.01,2")]double ShootCooldown = 0.6;
		Resource DialogueResource;
		string dialogue = "start";
		public double ShootCooldownV;
		double buff = 1;
		Vector2 MouseDirection;
		Vector2 direction;
		Node2D Bow;
		PackedScene bulletInstance;
		public Sprite2D sprite;
		Vector2 velocity;
		float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	#endregion
	private float GetGravity(){
		 return velocity.Y < .0f ? jumpGravity : fallGravity;
	}
	public override void _Ready(){
		DialogueResource = ResourceLoader.Load("res://dialogue/dia.dialogue");
		ShootCooldownV =  ShootCooldown;
		jumpVelocity = ((2.0f*jumpheight)/jumpTimeToPeak)*-1.0f;   // подстройка значений прыжка
		jumpGravity = ((-2.0f*jumpheight)/(jumpTimeToPeak*jumpTimeToPeak))*-1.0f;
		fallGravity = ((-2.0f*jumpheight)/(jumpTimeToDescent*jumpTimeToPeak))*-1.0f;
		LeftWall = GetNode<RayCast2D>("Left");
		RightWall = GetNode<RayCast2D>("Right");
	   sprite = GetNode<Sprite2D>("Sprite2D"); 
	   GetNode<TextureProgressBar>("TextureProgressBar").MaxValue = ShootCooldown;
	   Bow = GetNode<Node2D>("Bow");
	   bulletInstance = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	}
	public override void _Process(double delta)
	{
		LastOnGroundTime -=delta;
		wallJumpLeftTime -=delta;
		wallJumpRightTime -=delta;
		LastJump-=delta;
		if (ShootCooldownV != 0){
		   if(ShootCooldownV == ShootCooldown){
				CircleVal = 0;
				GetNode<TextureProgressBar>("TextureProgressBar").Visible = true;
		   }
		   GetNode<TextureProgressBar>("TextureProgressBar").Value = CircleVal;
		   ShootCooldownV -=delta;
		   CircleVal +=delta;
		   if (CircleVal >= ShootCooldown){
				GetNode<TextureProgressBar>("TextureProgressBar").Visible = false;
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
		var velocity = Velocity;
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

		if (Input.IsActionJustPressed("Jump")){
			LastJump=0.1005;
		}
		if(Input.IsActionJustPressed("ui_filedialog_show_hidden")){
			GetTree().Paused = true;
			Action();
		}
		
		direction.X = Input.GetAxis("Left", "Right");
		direction.Y = Input.GetAxis("Up", "Down");
		if(LeftWall.IsColliding()){
			wallJumpLeftTime = coyoteTime;
		}else if(RightWall.IsColliding()){
			wallJumpRightTime = coyoteTime;
		}
		if (!IsOnFloor()){
			if(velocity.Y<=450){
				if(!IsJumping&&((LeftWall.IsColliding()&&Input.IsActionPressed("Left"))||(RightWall.IsColliding()&&Input.IsActionPressed("Right")))){
				
					velocity.Y= 50;
				} else{
					velocity.Y+= GetGravity()*(float)delta;
					}
			}
		}else{
			LastOnGroundTime=coyoteTime;
		}
		if(LastJump>0&&LastOnGroundTime>0){// прыжок
				LastOnGroundTime=0;
				IsJumping = true;
				IsJumping2 = true;
				TimeJump =40;
				TimeJump2 =17;
				velocity.Y = jumpVelocity;		
			}
		if(!IsJumping2&&LastJump>0){ // прыжок от стены
			 if ( Input.IsActionPressed("Right") && wallJumpLeftTime>0){
				velocity.X-=jumpVelocity/2f;
				velocity.Y=jumpVelocity;
			 }else if (Input.IsActionPressed("Left") && wallJumpRightTime>0) {
				velocity.X+=jumpVelocity/2f;
				velocity.Y=jumpVelocity;	    
			 }
		}
		  if (direction.X != 0)  {    //движение ,работает через направление умноженную на скорость и по тихоньку ускоряется или замедляется
			velocity.X = Mathf.Lerp(velocity.X,direction.X*Speed,0.5f);
		  }else{
			velocity.X = Mathf.Lerp(velocity.X,0,0.3f);
		  }
		 if (Input.IsActionJustPressed("Lbm")&&ShootCooldownV<=0){   // выстрел персонажа
			  LastOnGroundTime=0;
			  wallJumpLeftTime=0;
			  wallJumpRightTime=0;
			  ShootCooldownV = ShootCooldown*buff;
			  var bullet =  (CharacterBody2D)bulletInstance.Instantiate();
			  bullet.GlobalPosition = GetNode<Node2D>("Bow/Node2D").GlobalPosition;
			  bullet.RotationDegrees= GetNode<Node2D>("Bow/Node2D").GlobalRotationDegrees;
			  if(Bow.ShowBehindParent == true){
					bullet.ShowBehindParent = true;
			  }
			  GetNode<SoundPlayer>("/root/SoundPlayer").PlaySound();
			  GetTree().Root.GetNode<Node2D>("Test1/CameraProxy").AddSibling(bullet);
			  var MouseDir = GlobalPosition.DirectionTo(GetGlobalMousePosition());
			  velocity.X -= (MouseDir.X * (float)recoilX);
			  velocity.Y -= (MouseDir.Y * (float)recoilY); 
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	private void Action(){
		var ballon = (CanvasLayer)BallonX.Instantiate();
		var balloon = (BalloonT)ballon;
		GetTree().CurrentScene.AddChild(ballon);
		balloon.Start(DialogueResource,dialogue);
	}
}
