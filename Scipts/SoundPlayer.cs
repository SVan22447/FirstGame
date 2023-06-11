using Godot;
using System;

public partial class SoundPlayer : Node{
	AudioStreamPlayer audioStreamPlayer;
	public override void _Ready(){
		audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioPlayers/Bow");
	}
	public void PlaySound(){
	   audioStreamPlayer.Play();
	}
}
