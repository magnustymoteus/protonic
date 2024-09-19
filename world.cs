using Godot;
using System;
using System.Diagnostics;
using protonic;
using System.Collections.Generic;

public partial class world : Node2D
{
	[Export] private Control buildingUI;
	[Export] private TextureRect textureRect;
	[Export] private ColorRect colorRect;

	private bool isBuilding;

	private string currentComponent;

	public Component[] placedComponents;

	public ComponentFactory factory;
		
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
	
	public void switchComponent(string componentPath)
	{
		currentComponent = "components/" + componentPath + "/disconnected.png";
		Texture2D texture = ResourceLoader.Load<Texture2D>(currentComponent);
		textureRect.SetTexture(texture);
		colorRect.SetSize(texture.GetSize());
	}
	
	public void clickedOnMap(Vector2 position) {
		
		if (isBuilding && currentComponent != null)
		{
			TextureRect textureRect = new TextureRect();
			textureRect.SetTexture(ResourceLoader.Load<Texture2D>(currentComponent));
			textureRect.SetPosition(position);
			AddChild(textureRect);
		}
	}
}
