using Godot;
using protonic;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;


public partial class world : Node2D
{
	[Export] private Control buildingUI;
	
	[Export] private TextureRect textureRect;
	[Export] private ColorRect colorRect;
	
	[Export] private TileMapLayer tileMap;
	
    [Export] private RichTextLabel currentInfoText;
    [Export] private ColorRect infoRect;
    private bool fadeInInfo = false;

	private bool isBuilding;

	private string currentComponentPath;

	public Dictionary<Tuple<int, int>, Component> map = new Dictionary<Tuple<int, int>, Component>();

	public ComponentFactory factory;
		
	public override void _Ready()
	{
		isBuilding = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (fadeInInfo)
		{
			Godot.Color color = infoRect.GetModulate();
			color.A = 0.8f;
			infoRect.SetModulate(infoRect.GetModulate().Lerp(color, 2.0f*(float)delta));
			if (infoRect.GetModulate().A >= 0.8f) fadeInInfo = false;
		}
	}
	public void PopupInfo()
	{
		SetOpacity(infoRect, 0.0f);
		currentInfoText.SetText(GetComponentTextFile("info.txt"));
		infoRect.GetParent<Control>().SetVisible(true);
		fadeInInfo = true;
	}
	public void SetOpacity(ColorRect currentColorRect, float opacity)
	{
		Godot.Color color = currentColorRect.GetModulate();
		color.A = opacity;
		currentColorRect.SetModulate(color);
	}
	
    public bool HasFile(string path, string fileName)
    {
        using var file = FileAccess.Open(path+"/"+fileName,FileAccess.ModeFlags.Read);
        return file != null;
    }
    
    public string LoadFromFile(string path)
    {
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null) return "";
        return file.GetAsText();
    }

    public string GetComponentTextFile(string file)
    {
	    string[] currentPathArr = currentComponentPath.Split('/');

	    while (!HasFile(currentPathArr.Join("/"), file) && currentPathArr.Length > 1)
	    {
		    currentPathArr = currentPathArr.Take(currentPathArr.Length-1).ToArray();
	    }

	    return LoadFromFile(currentPathArr.Join("/") + "/" + file);
    }

	public void switchBuildingMode()
	{
		isBuilding = !isBuilding;
		buildingUI.SetVisible(isBuilding);
	}
	
	public void switchComponent(string componentPath)
	{
		currentComponentPath = "components/" + componentPath + "/disconnected.png";
		Texture2D texture = ResourceLoader.Load<Texture2D>(currentComponentPath);
		textureRect.SetTexture(texture);
		colorRect.SetSize(texture.GetSize());
		PopupInfo();
	}

	public bool CanPlaceComponent(Component component)
	{
		bool XSmaller = component.beginPosition.X < component.endPosition.X;
		bool YSmaller = component.beginPosition.Y < component.endPosition.Y;
		int i = component.beginPosition.X;
		while (i != component.endPosition.X)
		{
			int j = component.beginPosition.Y;
			while (j != component.endPosition.Y)
			{
				Component actualComponent;
				if (map.TryGetValue(new Tuple<int, int>(i, j), out actualComponent) && actualComponent != null) return false;
				if (YSmaller) j++;
				else j--;
			}
			if (XSmaller) i++;
			else i--;
		}
		return true;
	}

	public void PlaceComponent(Component component)
	{
		TextureRect placedTextureRect = new TextureRect();
		placedTextureRect.SetTexture(ResourceLoader.Load<Texture2D>(component.name));
		placedTextureRect.SetPosition(textureRect.GetGlobalPosition());
		placedTextureRect.SetRotation(textureRect.GetParent<ColorRect>().GetRotation());
		AddChild(placedTextureRect);
		bool XSmaller = component.beginPosition.X < component.endPosition.X;
		bool YSmaller = component.beginPosition.Y < component.endPosition.Y;
		int i = component.beginPosition.X;
		while (i != component.endPosition.X)
		{
			int j = component.beginPosition.Y;
			while (j != component.endPosition.Y)
			{
				map.Add(new Tuple<int, int>(i, j), component);
				if (YSmaller) j++;
				else j--;
			}
			if (XSmaller) i++;
			else i--;
		}
	}
	public void clickedOnMap() {
		
		if (isBuilding && currentComponentPath != null)
		{
			Vector2 tileSize = tileMap.GetTileSet().GetTileSize();
			ColorRect parent = textureRect.GetParent<ColorRect>();
			Vector2I position = new Vector2I((int)(parent.GetPosition().X/tileSize.X), (int)(parent.GetPosition().Y/tileSize.Y));
			Vector2I size = new Vector2I((int)(textureRect.GetSize().X/tileSize.X), (int)(textureRect.GetSize().Y/tileSize.Y));
			float rotation = parent.GetRotation();
			Matrix2x2 rotationMatrix = new Matrix2x2(
				(int)Mathf.Cos(rotation), (int)-Mathf.Sin(rotation),
				(int)Mathf.Sin(rotation),  (int)Mathf.Cos(rotation)
			);
			size = rotationMatrix.Multiply(size);
			
			Component newComponent = new Component(currentComponentPath, position, size, rotation);
			
			if (CanPlaceComponent(newComponent)) PlaceComponent(newComponent);
		}
	}
}
