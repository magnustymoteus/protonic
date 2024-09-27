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
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("place"))
		{
			world.Call("ClickedOnMap");
		}
		else if (@event.IsActionPressed("delete"))
		{
			world.Call("DeleteComponent");
		}
	}
}
