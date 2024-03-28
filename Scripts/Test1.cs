using Godot;
using System;

public partial class Test1 : Node2D{
	public Vector2 RespawnZone;
	public Vector2 SaveZone;
	RomaGay Roma;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		Roma = GetNode<RomaGay>("/root/RomaGay");
		Roma.CollideCheckpoint +=(CheckpointPos)=>OnCollideCheckpoint(CheckpointPos);
	}
	public void OnCollideCheckpoint(Vector2 CheckpointPos){
		 	RespawnZone = CheckpointPos;
	}
	public override void _ExitTree(){
		Roma.CollideCheckpoint -= (CheckpointPos) => OnCollideCheckpoint(CheckpointPos);
	}
	public void OnSaveCheckS(Rid bodyRid, Node2D body,int bodyIndex,int localIndex){

	}
}
