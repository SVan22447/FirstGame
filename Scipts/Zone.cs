using Godot;
using System;

public partial class Zone : Area2D{

	public void CheckPoint(Node2D body){
		    GetNode<RomaGay>("/root/RomaGay").EmitSignal(RomaGay.SignalName.CollideCheckpoint,this.GlobalPosition);
	}
}
