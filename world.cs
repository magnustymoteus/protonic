using Godot;
using protonic;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using protonic.utils;


public partial class world : Node2D
{
	[Export] private Control buildingUI;
	
	[Export] private TextureRect textureRect;
	[Export] private ColorRect colorRect;
	
	[Export] private TileMapLayer tileMap;
	
	private bool isBuilding;

	private string currentComponentPath;

	public Dictionary<Vector2I, Component> ComponentMap = new Dictionary<Vector2I, Component>();

	public ComponentFactory factory;

	[Export] private Control infoUI;
		
	public override void _Ready()
	{
		isBuilding = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void Connect(string componentPath)
	{
		Vector2I globalPos = VectorConverter.Convert(textureRect.GetGlobalPosition());
		Vector2I size = VectorConverter.Convert(textureRect.GetSize());
		float rotation = textureRect.GetParent<ColorRect>().GetRotation();
		Component component = new Component(componentPath, globalPos, size, rotation);
		
		foreach (var availableConnectionFrom in component.availableConnections)
		{
			Component componentTo;
			Vector2I transformedPos = Component.ApplyConnectiveDirection(globalPos, availableConnectionFrom);
			ComponentMap.TryGetValue(transformedPos, out componentTo);
			if (componentTo != null)
			{
				SortedSet<ConnectiveDirection> availableConnectionsTo = componentTo.GetAvailableConnections();
				foreach (var availableConnectionTo in availableConnectionsTo)
				{
					if(availableConnectionTo == availableConnectionFrom.Item2) 
				}
			}
		}
	}

	public void UpdateComponentTexture()
	{
		string texturePath = Connect(currentComponentPath);
		Texture2D texture = ResourceLoader.Load<Texture2D>(texturePath);
		textureRect.SetTexture(texture);
		colorRect.SetSize(texture.GetSize());
	}
	
	public void OnTileChange()
	{
		if(isBuilding && currentComponentPath != null) UpdateComponentTexture();
	}
	
	public void SwitchBuildingMode()
	{
		isBuilding = !isBuilding;
		buildingUI.SetVisible(isBuilding);
	}
	
	public void SwitchComponent(string componentPath)
	{
		currentComponentPath = componentPath;
		UpdateComponentTexture();
		infoUI.Call("PopupInfo", currentComponentPath);
	}

	public bool CanPlaceComponent(Component component)
	{
		for (int i = component.beginPosition.X; i < component.endPosition.X; i++)
		{
			for (int j = component.beginPosition.Y; j < component.endPosition.Y; j++)
			{
				Component actualComponent;
				if (ComponentMap.TryGetValue(new Vector2I(i, j), out actualComponent) && actualComponent != null) return false;
			}
		}

		return true;
	}

	public void PlaceComponent(Component component)
	{
		TextureRect placedTextureRect = new TextureRect();
		placedTextureRect.SetTexture(ResourceLoader.Load<Texture2D>(Connect(currentComponentPath)));
		placedTextureRect.SetPosition(textureRect.GetGlobalPosition());
		placedTextureRect.SetRotation(textureRect.GetParent<ColorRect>().GetRotation());
		AddChild(placedTextureRect);
		for (int i = component.beginPosition.X; i < component.endPosition.X; i++)
		{
			for (int j = component.beginPosition.Y; j < component.endPosition.Y; j++)
			{
				ComponentMap.Add(new Vector2I(i, j), component);
			}
		}
	}
	public void ClickedOnMap()
	{
		if (isBuilding && currentComponentPath != null)
		{
			Vector2 tileSize = tileMap.GetTileSet().GetTileSize();
			ColorRect parent = textureRect.GetParent<ColorRect>();
			Vector2I position = VectorConverter.Convert(parent.GetPosition() / tileSize);
			Vector2I size = VectorConverter.Convert(textureRect.GetSize() / tileSize);
			float rotation = parent.GetRotation();
			size = Matrix2x2.GetRotationMatrix(rotation).Multiply(size);
			
			Component newComponent = new Component(currentComponentPath, position, size, rotation);
			if (CanPlaceComponent(newComponent)) PlaceComponent(newComponent);
		}
	}
}
