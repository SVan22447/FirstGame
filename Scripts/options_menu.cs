using Godot;
using System;

public partial class options_menu : Control{
	public CenterContainer Center;
	public BoxContainer Menu;
	Button RemappingMenu;
	public Button play;
	public HSlider MusicSliderVar;
	public HSlider SFXSliderVar;
	public CheckButton FullScreenButton;
	public RomaGay RomaH;
	public override void _Ready(){
		RomaH = GetNode<RomaGay>("/root/RomaGay");
		RemappingMenu= GetNode<Button>("CenterContainer/VBoxContainer/HBoxContainer3/Control/Button");
		MusicSliderVar =GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer/Control/HSlider");
		SFXSliderVar=GetNode<HSlider>("CenterContainer/VBoxContainer/HBoxContainer2/Control/HSlider");
		FullScreenButton =GetNode<CheckButton>("CenterContainer/VBoxContainer/HBoxContainer4/Control/CheckButton");
	}
	public void Update(){
		// if(){
		if(RomaH.isFullscreen){
			FullScreenButton.ButtonPressed=true;
		}else{
			FullScreenButton.ButtonPressed=false;
		}
		// }
		MusicSliderVar.Value =AudioServer.GetBusVolumeDb(1);
		SFXSliderVar.Value =  AudioServer.GetBusVolumeDb(2);
	}
	private void Focus(){
        GetNode<Button>("Control2/Button").GrabFocus();
	}
    public override void _Input(InputEvent @event){
           if (Input.IsActionJustReleased("FullScreenButton")){
			    Update();
		   }
    }
	public void FullscreenToggled(bool ScreenB){
        if(ScreenB){
			RomaH.Lastscreen=DisplayServer.WindowGetMode(DisplayServer.WindowGetCurrentScreen());
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
	public void InputRemapping(){
		GetNode<Button>("Control2/Button").GrabFocus();
		GetNode<CenterContainer>("CenterContainer").Visible=!GetNode<CenterContainer>("CenterContainer").Visible;
		GetNode<CenterContainer>("CenterContainer2").Visible=!GetNode<CenterContainer>("CenterContainer2").Visible;
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
		if(GetTree().CurrentScene.SceneFilePath != "res://Scenes/Levels/menu.tscn" ){
			if(!GetNode<CenterContainer>("CenterContainer2").Visible){
				Center.Visible = !Center.Visible;
				this.Visible = !this.Visible;
			}else{
				RemappingMenu.EmitSignal(Button.SignalName.Pressed);
			}
		}else{
		    if(!GetNode<CenterContainer>("CenterContainer2").Visible){
				Menu.Visible = !Menu.Visible;
				GetNode<Control>("/root/menu/Control").Visible = !GetNode<Control>("/root/menu/Control").Visible;
				play.GrabFocus();
		    }else{
				RemappingMenu.EmitSignal(Button.SignalName.Pressed);
			}
		}
	}
}
