using Godot;
using System;

public partial class FollowMouse : Node2D
{
    private Vector2I tileSize;
    private Vector2 mousePos;
    [Export] TileMapLayer tileMapLayer;

	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tileSize = tileMapLayer.TileSet.TileSize;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        mousePos = GetGlobalMousePosition();
        if (tileMapLayer != null)
        {
            Position = new Vector2(tileSize.X * Mathf.Ceil(mousePos.X/tileSize.X), tileSize.Y * Mathf.Ceil(mousePos.Y/tileSize.Y));
        }
        else Position = mousePos;
    }
}