using System.IO;
using Godot;
using protonic;
using protonic.utils;
using protonic.utils.TransferMatrix;
using Vector2 = Godot.Vector2;


public partial class World : Node2D
{
	[Export] public Control BuildingUI;

	[Export] public Sprite2D ElementHoverSprite;
	[Export] public ColorRect ColorHoverRect;
	
	[Export] public TileMapLayer TileMap;
	public Vector2 TileSize;

	public bool IsBuilding = false;

	public string CurrentElementPath;
	public string CurrentElementName;
	
	[Export] public Control InfoUI;

	public MapManager Map;
	public ActionManager ActionManager = ActionManager.GetInstance();
	
	private ElementFactory _elementFactory = new ElementFactory();

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

	public void UpdateElementTexture()
	{
		if (IsBuilding)
		{
			string texturePath = CurrentElementPath + "/texture.png";
			if (File.Exists(texturePath))
			{
				Texture2D texture = ResourceLoader.Load<Texture2D>(texturePath);
				ElementHoverSprite.SetTexture(texture);
				ElementHoverSprite.SetPosition(ElementHoverSprite.GetTexture().GetSize() / 2.0f);
				ColorHoverRect.SetSize(texture.GetSize());
			}
		}
		else
		{
			ElementHoverSprite.SetTexture(null);
			ColorHoverRect.SetSize(TileSize);
		}
	}
	
	public void OnTileChange()
	{
		if(CurrentElementPath != null) UpdateElementTexture();
	}
	
	public void SwitchBuildingMode()
	{
		IsBuilding = !IsBuilding;
		BuildingUI.SetVisible(IsBuilding);
	}
	
	public void SwitchElement(string elementName)
	{
		CurrentElementPath = "./components/particleAccelerator/elements"+elementName;
		CurrentElementName = elementName;
		UpdateElementTexture();
		InfoUI.Call("PopupInfo", CurrentElementPath);
		
	}


	public void DeleteElement()
	{
		Element element = Map.GetElement(VectorConverter.Convert(ElementHoverSprite.GetParent<ColorRect>().GetPosition()/TileSize));
		if(element != null) Map.DeleteElement(element);
	}
	public void ClickedOnMap()
	{
		FollowMouse parent = ElementHoverSprite.GetParent<FollowMouse>();
		Vector2I position = VectorConverter.Convert(parent.GetPosition() / TileSize);
		if (IsBuilding && CurrentElementPath != null)
		{
			Vector2I size = VectorConverter.Convert(ElementHoverSprite.GetRect().Size / TileSize);
			float rotation = parent.GetRotation();
			size = Matrix2x2.GetRotationMatrix(rotation).Multiply(size);

			Element newElement = _elementFactory.CreateElement(CurrentElementName, position, size, rotation);
			Map.AddElement(newElement);
		}
	}
	
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionPressed("undo")) ActionManager.Undo();
		else if(Input.IsActionPressed("redo")) ActionManager.Redo();
	}

}