using Godot;
using System;

public partial class Effect : Sprite2D
{
	public override void _Ready(){
		CreateTween().TweenProperty(this,"modulate:a", 0.0,0.35).SetTrans(Tween.TransitionType.Quart).Finished+=QueueFree;	
	}
}
