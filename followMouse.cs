using Godot;
using System;

public partial class followMouse : ColorRect
{
	[Export] TileMapLayer tileMapLayer;

	private Vector2I tileSize;
	private Vector2 mousePos;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileSize = tileMapLayer.TileSet.TileSize;
		Size = tileSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
		if (tileMapLayer != null)
		{
			Position = new Vector2(tileSize.X * Mathf.Floor(mousePos.X/tileSize.X), tileSize.Y * Mathf.Floor(mousePos.Y/tileSize.Y));
		}
		else Position = mousePos;
	}
}
