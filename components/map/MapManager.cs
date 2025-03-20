using System;

namespace protonic;
using protonic.utils;
using System.Collections.Generic;
using Godot;

public class MapManager 
{
    public Dictionary<Vector2I, Element> Elements = new Dictionary<Vector2I, Element>();

    public World root;
    public MapManager(World root)
    {
	    this.root = root;
    }
    public Element GetElement(Vector2I tile)
    {
	    Elements.TryGetValue(tile, out var result);
        return result;
    }

    public bool ElementExists(Vector2I tile)
    {
        return GetElement(tile) != null;
    }
    public void AddElement(Element element, bool addToActions=true)
    {
	    UpdateTexture(element);
	    if (!CanPlaceElement(element)) return;
	    for (int i = element.beginPosition.X; i < element.endPosition.X; i++)
	    {
		    for (int j = element.beginPosition.Y; j < element.endPosition.Y; j++)
		    {
			    Elements.Add(new Vector2I(i, j), element);
		    }
	    }

	    if (addToActions)
	    {
		    Action action = new Action(
			    "Add Element",
			    () => AddElement(element, false)
			    );
		    action.UndoAction = new Action(
			    "Remove Element",
			    () => DeleteElement(element, false)
		    );
		    root.ActionManager.AddAction(action);
	    }
    }

    public void DeleteElement(Element element, bool addToActions=true)
    {
        root.RemoveChild(element);
        
        for (int i = element.beginPosition.X; i < element.endPosition.X; i++)
        {
	        for (int j = element.beginPosition.Y; j < element.endPosition.Y; j++)
	        {
		        Elements.Remove(new Vector2I(i, j));
	        }
        }
        if (addToActions)
        {
	        Action action = new Action(
		        "Remove Element",
		        () => DeleteElement(element, false)
	        );
	        action.UndoAction = new Action(
		        "Add Element",
		        () => AddElement(element, false)
	        );
	        root.ActionManager.AddAction(action);
        }
    }
    
    
    
    public void UpdateTexture(Element element)
    {
	    bool isNew = GetElement(element.beginPosition) == null;
	    if (isNew)
	    {
		    element.SetTextureFilter(CanvasItem.TextureFilterEnum.Nearest);
		    element.SetCentered(true);
		    element.SetRotation(element.rotation);
		    element.SetPosition((VectorConverter.Convert(element.beginPosition+element.endPosition)/2.0f)*root.TileSize);
	    }
	    Texture2D newTexture = ResourceLoader.Load<Texture2D>(element.GetTexturePath());
	    element.SetTexture(newTexture);
	    if (isNew)
	    {
		    root.AddChild(element);
	    }
    }

    public bool CanPlaceElement(Element element)
    {
	    for (int i = element.beginPosition.X; i < element.endPosition.X; i++)
		{
			for (int j = element.beginPosition.Y; j < element.endPosition.Y; j++)
			{
				if (ElementExists(new Vector2I(i, j))) return false;
			}
		}
		return true;
    }
}