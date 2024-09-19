using Godot;
using System;

public partial class FollowMouse : ColorRect
{
    private Vector2I tileSize;
    private Vector2 mousePos;
    [Export] private TileMapLayer tileMapLayer;

    public override void _Ready()
    {
        tileSize = tileMapLayer.TileSet.TileSize;
        PivotOffset = tileSize / 2;
    }

    public Vector2 GetTiledPosition()
    {
        return new Vector2(tileSize.X * Mathf.Ceil(mousePos.X / tileSize.X),
            tileSize.Y * Mathf.Ceil(mousePos.Y / tileSize.Y));
    }
    public override void _Process(double delta)
    {
        Vector2 newMousePos = GetGlobalMousePosition();
        if (mousePos != newMousePos)
        {
            mousePos = newMousePos;
            Vector2 tiledPosition = GetTiledPosition();
            if (Position != tiledPosition) Position = GetTiledPosition();
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("rotate"))
        {
            float rotation = (float)Math.PI / 2.0f;
            SetRotation(GetRotation() + rotation);
        }
    }
}