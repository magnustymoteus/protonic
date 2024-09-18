using Godot;
using System;
using System.Diagnostics;

public partial class world : Node2D
{
	[Export] private Control buildingUI;
	[Export] private TextureRect textureRect;
	[Export] private ColorRect colorRect;

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
	public void switchComponent(string componentPath) {
		GD.Print("switched to ", componentPath);
		Texture2D texture = ResourceLoader.Load<Texture2D>("components/"+componentPath+"/disconnected.png");
		textureRect.SetTexture(texture);
		colorRect.SetSize(texture.GetSize());
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

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("rotate"))
		{
			float rotation = (float)Math.PI / 2.0f;
			textureRect.Rotation += rotation;
			colorRect.Rotation += rotation;
		}
	}
}
