using Godot;
using System;

public partial class Map : ColorRect
{
	[Export] private Node2D world;

	[Export] private ColorRect followRect;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("place"))
		{
			world.Call("ClickedOnMap");
		}
	}
	
}
