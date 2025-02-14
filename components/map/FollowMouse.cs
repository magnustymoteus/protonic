using Godot;
using System;

public partial class FollowMouse : ColorRect
{
    private Vector2I tileSize;
    [Export] private TileMapLayer tileMapLayer;
    [Export] private Node2D world;

    public override void _Ready()
    {
        tileSize = tileMapLayer.TileSet.TileSize;
        PivotOffset = tileSize / 2;
    }

    public Vector2 GetTiledPosition()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        return new Vector2(tileSize.X * Mathf.Floor(mousePos.X / tileSize.X),
            tileSize.Y * Mathf.Floor(mousePos.Y / tileSize.Y));
    }
    public override void _Process(double delta)
    {
        if (Position != GetTiledPosition())
        {
            world.Call("OnTileChange");
        }
        Position = GetTiledPosition();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("rotate"))
        {
            SetRotation(GetRotation()+Mathf.Pi / 2.0f);
        }
    }
}