using Godot;
using System;

public partial class options_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	[Signal]public delegate void UpdateVEventHandler();
	public CenterContainer Center;
	public BoxContainer Menu;
	public Button play;
	public RomaGay RomaH;
	public override void _Ready(){
		RomaH = GetNode<RomaGay>("/root/RomaGay");
	}
	public void Update(){
		if(RomaH.isFullscreen==true){
			GetNode<CheckButton>("CenterContainer/VBoxContainer/HBoxContainer4/Control/CheckButton").ButtonPressed=true;
		}else{
			GetNode<CheckButton>("CenterContainer/VBoxContainer/HBoxContainer4/Control/CheckButton").ButtonPressed=false;
		}
		GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer/Control/HSlider").Value =AudioServer.GetBusVolumeDb(1);
		GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer2/Control/HSlider").Value =  AudioServer.GetBusVolumeDb(2);
	}
    public override void _Input(InputEvent @event){
           if (Input.IsActionJustReleased("FullScreenButton")){
			    EmitSignal(SignalName.UpdateV);
		   }
    }
	public void FullscreenToggled(bool ScreenB){
        if(ScreenB ==true){
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			RomaH.isFullscreen=ScreenB;
		}else {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			RomaH.isFullscreen=ScreenB;
		}
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
