using Godot;
using System;

public partial class CameraProxy : Node2D{
    [Export]public Node2D target;
    [Export(PropertyHint.Range,"0f,10f")] float DistanceWeight=5f;
    [Export(PropertyHint.Range,"0f,0.5f")] float CameraSpeed=0.162f;
	public override void _PhysicsProcess(double delta){
        Vector2 pos = target.GlobalPosition;
        if (target is Player){
            Player _player = target as Player;
            pos += (GetGlobalMousePosition()-_player.GlobalPosition)/DistanceWeight;
            GlobalPosition = GlobalPosition.Lerp(pos, CameraSpeed);
	      }
    }
}
