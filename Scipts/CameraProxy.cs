using Godot;
using System;

public partial class CameraProxy : Node2D
{
    [Export]public Node2D target;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
        Vector2 pos = target.GlobalPosition;
        if (target is CharacterBody2D)
        {
            CharacterBody2D player = target as CharacterBody2D;
            pos += player.sprite.FlipH ? new Vector2(0, 0) : new Vector2(0, 0);
            // if (!player.IsOnFloor() && !player.IsOnWall()&& player.Velocity.Y < 450)
            // {
            //     pos = pos with {Y = Position.Y};
            // }
        }
        Position = Position.Lerp(pos, 0.172f);
	}
}
