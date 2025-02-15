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
	    if (!CanPlaceElement(element)) return;
	    var affectedElements = Connect(element);
	    foreach (Element affectedElement in affectedElements)
	    {
		    UpdateTexture(affectedElement);
		    affectedElement.ClearConnectionArrows(root);
		    affectedElement.PlaceConnectionArrows(root.TileSize, root);
	    }
	    if (affectedElements.Count == 0) UpdateTexture(element);
	    for (int i = element.beginPosition.X; i < element.endPosition.X; i++)
	    {
		    for (int j = element.beginPosition.Y; j < element.endPosition.Y; j++)
		    {
			    Elements.Add(new Vector2I(i, j), element);
		    }
	    }
		element.PlaceConnectionArrows(root.TileSize, root);

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
        element.ClearConnectionArrows(root);
        
        for (int i = element.beginPosition.X; i < element.endPosition.X; i++)
        {
	        for (int j = element.beginPosition.Y; j < element.endPosition.Y; j++)
	        {
		        Elements.Remove(new Vector2I(i, j));
	        }
        }
        
        foreach (var connection in element.occupiedConnections)
        {
	        foreach (var direction in connection.Value)
	        {
		        Vector2I targetPos = element.rectBeginPosition+Matrix2x2.GetRotationMatrix(element.rotation).Multiply(connection.Key)+Element.Convert(direction);
		        Element targetElement = GetElement(targetPos);
		        targetPos = targetElement.ConvertTargetToConnection(targetPos-Element.Convert(direction), 
			        Element.Convert(-Element.Convert(direction)));
		        targetElement.RemoveConnection(targetPos, Element.Convert(-Element.Convert(direction)));
		        targetElement.ClearConnectionArrows(root);
		        targetElement.PlaceConnectionArrows(root.TileSize, root);
		        UpdateTexture(targetElement);

		        element.availableConnections[connection.Key].Add(element.UndoRotation(direction));
	        }
        }
        
        element.occupiedConnections.Clear();

        
        
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
    
    public HashSet<Element> Connect(Element sourceElement)
    {
	    HashSet<Element> affectedElements = new HashSet<Element>();
	    foreach (var targetFrom in sourceElement.GetAvailableConnectionTargets())
	    {
		    Element targetElement = GetElement(targetFrom.Item2);
		    if (targetElement != null)
		    {
			    Vector2I sourceConnectPos = targetFrom.Item2 - Element.Convert(targetFrom.Item1);
			    if (targetElement.CanConnect(sourceConnectPos, sourceElement.path))
			    {
				    sourceElement.AddElementConnection(targetFrom.Item2, targetElement);
				    targetElement.AddElementConnection(sourceConnectPos, sourceElement);
				    
				    Vector2I sourcePos = sourceElement.ConvertTargetToConnection(targetFrom.Item2, targetFrom.Item1);
				    Vector2I targetPos = targetElement.ConvertTargetToConnection(sourceConnectPos, Element.Convert(-Element.Convert(targetFrom.Item1)));
				    
				    sourceElement.AddConnection(sourcePos, targetFrom.Item1);
				    targetElement.AddConnection(targetPos, Element.Convert(-Element.Convert(targetFrom.Item1)));
				    
				    affectedElements.Add(sourceElement);
				    affectedElements.Add(targetElement);
			    }
		    }
	    }
	    return affectedElements;
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