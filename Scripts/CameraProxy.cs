using Godot;
using System;

public partial class CameraProxy : Node2D{
    [Export]public Node2D target;
	public override void _PhysicsProcess(double delta){
        Vector2 pos = target.GlobalPosition;
        if (target is Player){
            Player _player = target as Player;
            pos += (GetGlobalMousePosition()-_player.GlobalPosition)/4;
            GlobalPosition = GlobalPosition.Lerp(pos, 0.162f);
	      }
    }
}
