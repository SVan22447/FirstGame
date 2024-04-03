
using System.IO;
using Godot;

public partial class PlayerStats : Resource{
    [Export]public bool Played =false;
    [Export]public int Lives = 5;
    [Export]public Vector2 pos=new Vector2(-364,418);
    [Export]public string pathLevel="res://Scenes/Levels/Game.tscn";
}
