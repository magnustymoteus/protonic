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
	public Dictionary<Vector2I, TextureRect> TextureMap = new Dictionary<Vector2I, TextureRect>();

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

	public HashSet<Component> Connect(Component sourceComponent)
	{
		HashSet<Component> affectedComponents = new HashSet<Component>();
		foreach (var targetFrom in sourceComponent.GetAvailableConnectionTargets())
		{
			Component targetComponent;
			ComponentMap.TryGetValue(targetFrom.Item2, out targetComponent);
			if (targetComponent != null)
			{
				Vector2I sourceConnectPos = targetFrom.Item2 - Component.Convert(targetFrom.Item1);
				if (targetComponent.CanConnect(sourceConnectPos))
				{
					sourceComponent.AddConnection(sourceConnectPos- sourceComponent.beginPosition, targetFrom.Item1);
					targetComponent.AddConnection(targetFrom.Item2-targetComponent.beginPosition, Component.Convert(-Component.Convert(targetFrom.Item1)));
					affectedComponents.Add(sourceComponent);
					affectedComponents.Add(targetComponent);
				}
			}
		}

		return affectedComponents;
	}

	public void UpdateComponentTexture()
	{
		string texturePath = currentComponentPath + "/disconnected.png";
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

	public void UpdateTexture(Component component)
	{
		TextureRect placedTextureRect;
		GD.Print("position", component.beginPosition);
		TextureMap.TryGetValue(component.beginPosition, out placedTextureRect);
		bool isNew = placedTextureRect == null;
		if (isNew)
		{
			placedTextureRect = new TextureRect();
			placedTextureRect.SetPosition(textureRect.GetGlobalPosition());
			placedTextureRect.SetRotation(textureRect.GetParent<ColorRect>().GetRotation());
		}
		placedTextureRect.SetTexture(ResourceLoader.Load<Texture2D>(component.GetTexturePath()));
		if (isNew)
		{
			AddChild(placedTextureRect);
			TextureMap.Add(component.beginPosition, placedTextureRect);
		}
	}

	public void PlaceComponent(Component component)
	{
		var affectedComponents = Connect(component);
		foreach (Component affectedComponent in affectedComponents)
		{
			UpdateTexture(affectedComponent);
		}
		if (affectedComponents.Count == 0) UpdateTexture(component);
		
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
