using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
	[Export] private Node2D world;
	// Called when the node enters the scene tree for the first time.
	private Vector2 tileSize;
	public override void _Ready()
	{
		tileSize = GetTileSet().TileSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			var position = new Vector2(tileSize.X * Mathf.Floor(eventMouseButton.Position.X / tileSize.X),
				tileSize.Y * Mathf.Floor(eventMouseButton.Position.Y / tileSize.Y));
			world.Call("clickedOnMap", position);
		}
	}
}
