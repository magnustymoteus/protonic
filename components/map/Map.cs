using Godot;
using System;

public partial class Map : ColorRect
{
	[Export] private World world;

	[Export] private ColorRect followRect;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionPressed("place"))
		{
			world.ClickedOnMap();
		}
		else if (Input.IsActionPressed("delete"))
		{
			world.DeleteElement();
		}
	}
}
