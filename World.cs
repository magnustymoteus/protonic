using Godot;
using protonic;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using protonic.utils;
using Color = Godot.Color;


public partial class World : Node2D
{
	[Export] public Control BuildingUI;
	
	[Export] public TextureRect ComponentHoverRect;
	[Export] public ColorRect ColorHoverRect;
	
	[Export] public TileMapLayer TileMap;
	public Vector2 TileSize;

	public bool IsBuilding = false;

	public string CurrentComponentPath;
	
	[Export] public Control InfoUI;
	[Export] public Control UpgradeUI;

	public MapManager Map;
	public ActionManager ActionManager = ActionManager.GetInstance();

	public string CurrentLevelPath;
	
	public World(int Level)
	{
		CurrentLevelPath = "./levels/level_" + Level + ".json";
;	}

	public World() : this(1)
	{ }

	public override void _Ready()
	{
		TileSize = TileMap.GetTileSet().GetTileSize();
		Map = new MapManager(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void UpdateComponentTexture()
	{
		if (IsBuilding)
		{
			string texturePath = CurrentComponentPath + "/disconnected.png";
			Texture2D texture = ResourceLoader.Load<Texture2D>(texturePath);
			ComponentHoverRect.SetTexture(texture);
			ColorHoverRect.SetSize(texture.GetSize());
		}
		else
		{
			ComponentHoverRect.SetTexture(null);
			ColorHoverRect.SetSize(TileSize);
		}
	}
	
	public void OnTileChange()
	{
		if(CurrentComponentPath != null) UpdateComponentTexture();
	}
	
	public void SwitchBuildingMode()
	{
		IsBuilding = !IsBuilding;
		BuildingUI.SetVisible(IsBuilding);
	}
	
	public void SwitchComponent(string componentPath)
	{
		CurrentComponentPath = componentPath;
		UpdateComponentTexture();
		InfoUI.Call("PopupInfo", CurrentComponentPath);
	}


	public void DeleteComponent()
	{
		Component component = Map.GetComponent(VectorConverter.Convert(ComponentHoverRect.GetParent<ColorRect>().GetPosition()/TileSize));
		if(component != null) Map.DeleteComponent(component);
		
	}
	public void ClickedOnMap()
	{
		ColorRect parent = ComponentHoverRect.GetParent<ColorRect>();
		Vector2I position = VectorConverter.Convert(parent.GetPosition() / TileSize);
		if (!IsBuilding && Map.ComponentExists(position) && !UpgradeUI.IsVisible())
		{
			UpgradeUI.SetGlobalPosition(position*TileSize);
			UpgradeUI.SetVisible(true);
		}
		else if (IsBuilding && CurrentComponentPath != null)
		{
			Vector2I size = VectorConverter.Convert(ComponentHoverRect.GetSize() / TileSize);
			float rotation = parent.GetRotation();
			size = Matrix2x2.GetRotationMatrix(rotation).Multiply(size);
			
			Component newComponent = new Component(CurrentComponentPath, position, size, rotation);
			Map.AddComponent(newComponent);
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionPressed("undo")) ActionManager.Undo();
		else if(Input.IsActionPressed("redo")) ActionManager.Redo();
	}
}
