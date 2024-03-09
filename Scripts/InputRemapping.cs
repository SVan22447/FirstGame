using Godot;
using System;
using System.Threading;

public partial class InputRemapping : Button{
	public String action;
	private int Wait=0;
	string IconText;
	public override void _Ready(){
		action=Name;
		SetProcessUnhandledKeyInput(false);
		DisplayKey();

	}
	private void DisplayKey(){
	    IconText = InputMap.ActionGetEvents(action)[0].AsText();
		Icon = GD.Load<CompressedTexture2D>($"res://addons/ActionIcon/Keyboard/{IconText}.png");
	}
    private void _OnActionButtonToggled(bool ButtonPressed){
			// if(Input.IsActionPressed("ui_accept")&&Wait<1){
			// 	Wait=0;
			// }else{
			// 	Wait=1;
			// }
			// // if(Input.IsActionJustReleased("ui_accept")){
			// // 	Wait=1;
			// // }
			// if(Wait==1){
			// 	Wait=0;
			SetProcessUnhandledKeyInput(ButtonPressed);
			// }
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
	public void RemapKey(InputEvent @event){
		InputMap.ActionEraseEvents(action);
		InputMap.ActionAddEvent(action, @event);
		Text=null;
	}
	private void Reset(){
	}
}
