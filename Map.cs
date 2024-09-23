using Godot;
using System;

public partial class Map : ColorRect
{
	[Export] private Node2D world;

	[Export] private ColorRect followRect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("place"))
		{
			world.Call("ClickedOnMap");
		}
	}
	
}
