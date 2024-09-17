using Godot;
using System;

public partial class FollowRect : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	private Vector2I tileSize;
	[Export] private TileMapLayer tileMapLayer;

	public override void _Ready()
	{
		tileSize = tileMapLayer.TileSet.TileSize;
		Size = tileSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
