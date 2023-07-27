using Godot;
using System;

public partial class InputRemapping : Button
{
	// Called when the node enters the scene tree for the first time.
	String action;
	public override void _Ready(){
		action=Name;
		SetProcessUnhandledKeyInput(false);
		DisplayKey();

	}
	private void DisplayKey(){
		string IconText = InputMap.ActionGetEvents(action)[0].AsText();
		Icon = GD.Load<CompressedTexture2D>($"res://addons/ActionIcon/Keyboard/{IconText}.png");
	}
    private void _OnActionButtonToggled(bool ButtonPressed){
		SetProcessUnhandledKeyInput(ButtonPressed);
		if(ButtonPressed){
			Text= "...";
		}else{
			DisplayKey();
		}
	}
    public override void _UnhandledKeyInput(InputEvent @event){
        RemapKey(@event);
		ButtonPressed=false;
    }
	private void RemapKey(InputEvent @event){
		InputMap.ActionEraseEvents(action);
		InputMap.ActionAddEvent(action, @event);
		Text=null;
	}
}
