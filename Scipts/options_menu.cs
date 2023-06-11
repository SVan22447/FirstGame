using Godot;
using System;

public partial class options_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public CenterContainer Center;
	public BoxContainer Menu;
	public Button play;
	public override void _Ready(){


	}
	public void Update(){
		GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer/Control/HSlider").Value =AudioServer.GetBusVolumeDb(1);
		GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer2/Control/HSlider").Value =  AudioServer.GetBusVolumeDb(2);
	}
	public void MusicSlider(float value){
		volume(1,value);
	}
	public void SFXSlider(float value){
		volume(2,value);
	}
	public void SFXend(bool Change){
		GetNode<AudioStreamPlayer>("/root/SoundPlayer/AudioPlayers/Bow").Play();
	}
	private void volume(int BusIndex,float value){
		if(value == -45){
		   AudioServer.SetBusMute(BusIndex,true);
		}else{
		   AudioServer.SetBusMute(BusIndex,false);
		}
		   AudioServer.SetBusVolumeDb(BusIndex,value);
	}
	public void BackCenter(){
		if(GetTree().CurrentScene.SceneFilePath != "res://Scenes/menu.tscn" ){
		  Center.Visible = !Center.Visible;
		  this.Visible = !this.Visible;
		}else{
		   Menu.Visible = !Menu.Visible;
		   GetNode<Control>("/root/menu/Control").Visible = !GetNode<Control>("/root/menu/Control").Visible;
		   play.GrabFocus();
		}
	}
}
