using Godot;
using System;
using System.Diagnostics;

public partial class world : Node2D
{
	[Export] private Control buildingUI;

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
	public void switchComponent(string componentName) {
		GD.Print("switched to ", componentName);
	}

	public string LoadFromFile()
	{
		using var file = FileAccess.Open("user://save_game.dat", FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		return content;
	}
	
	public void clickedOnMap(Vector2 position) {
		if (isBuilding)
		{
			GD.Print("place on ", position);
		}
	}
}
