using Godot;
using System;

public partial class SoundPlayer : Node{
	AudioStreamPlayer audioStreamPlayer;
	AudioStreamPlayer HurtSound;
	public override void _Ready(){
		audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioPlayers/Bow");
        HurtSound = GetNode<AudioStreamPlayer>("AudioPlayers/Hurt");
	}

	public void PlaySound(){
	   audioStreamPlayer.Play();
	}
	public void HurtPlayedPlayer(){
	   HurtSound.Play();
	}
}
