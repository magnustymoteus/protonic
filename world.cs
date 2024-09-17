using Godot;
using System;
using System.Diagnostics;

public partial class world : Node2D
{
	[Export] private Node2D buildingUI;

	private bool isBuilding;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		isBuilding = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void switchBuildingMode()
	{
		isBuilding = !isBuilding;
		buildingUI.SetVisible(isBuilding);
	}
	public void switchComponent(int id) {
		GD.Print("switched to ", id);
	}
	
	public void clickedOnMap(Vector2 position) {
		if (isBuilding)
		{
			GD.Print("place on ", position);
		}
	}
}
