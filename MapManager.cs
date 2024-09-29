namespace protonic;
using protonic.utils;
using System.Collections.Generic;
using Godot;

public class MapManager 
{
    public Dictionary<Vector2I, Component> Components = new Dictionary<Vector2I, Component>();
    public Dictionary<Vector2I, TextureRect> Textures = new Dictionary<Vector2I, TextureRect>();

    protected World root;
    public MapManager(World root)
    {
	    this.root = root;
    }
    public Component GetComponent(Vector2I tile)
    {
        Component result;
        this.Components.TryGetValue(tile, out result);
        return result;
    }
    
    public TextureRect GetTexture(Vector2I tile)
    {
	    TextureRect result;
	    this.Textures.TryGetValue(tile, out result);
	    return result;
    }

    public bool ComponentExists(Vector2I tile)
    {
        return GetComponent(tile) != null;
    }
    public void AddComponent(Component component, bool addToActions=true)
    {
	    var affectedComponents = Connect(component);
	    foreach (Component affectedComponent in affectedComponents)
	    {
		    UpdateTexture(affectedComponent);
		    affectedComponent.ClearConnectionArrows(root);
		    affectedComponent.PlaceConnectionArrows(root.TileSize, root);
	    }
	    if (affectedComponents.Count == 0) UpdateTexture(component);
		
	    for (int i = component.beginPosition.X; i < component.endPosition.X; i++)
	    {
		    for (int j = component.beginPosition.Y; j < component.endPosition.Y; j++)
		    {
			    Components.Add(new Vector2I(i, j), component);
		    }
	    }


	    if (addToActions)
	    {
		    Action action = new Action(
			    "Add Component",
			    () => AddComponent(component, false)
		    );
		    action.UndoAction = new Action(
			    "Remove Component",
			    () => DeleteComponent(component, false)
		    );
		    root.ActionManager.AddAction(action);
	    }
    }

    public void DeleteComponent(Component component, bool addToActions=true)
    {
        TextureRect rect = GetTexture(component.beginPosition);
        Textures.Remove(component.beginPosition);
        root.RemoveChild(rect);
        component.ClearConnectionArrows(root);
        
        for (int i = component.beginPosition.X; i < component.endPosition.X; i++)
        {
	        for (int j = component.beginPosition.Y; j < component.endPosition.Y; j++)
	        {
		        Components.Remove(new Vector2I(i, j));
	        }
        }
        
        foreach (var connection in component.connections)
        {
	        foreach (var direction in connection.Value)
	        {
		        Vector2I targetPos = component.beginPosition+connection.Key + Component.Convert(direction);
		        Component targetComponent = GetComponent(targetPos);
		        targetComponent.RemoveConnection(component.beginPosition+connection.Key+Component.Convert(direction)-targetComponent.beginPosition, Component.Convert(-Component.Convert(direction)));
		        targetComponent.ClearConnectionArrows(root);
		        targetComponent.PlaceConnectionArrows(root.TileSize, root);
		        UpdateTexture(targetComponent);
	        }
        }

        if (addToActions)
        {
	        Action action = new Action(
		        "Remove Component",
		        () => DeleteComponent(component, false)
	        );
	        action.UndoAction = new Action(
		        "Add Component",
		        () => AddComponent(component, false)
	        );
	        root.ActionManager.AddAction(action);
        }
    }
    
    public HashSet<Component> Connect(Component sourceComponent)
    {
	    HashSet<Component> affectedComponents = new HashSet<Component>();
	    foreach (var targetFrom in sourceComponent.GetAvailableConnectionTargets())
	    {
		    Component targetComponent = GetComponent(targetFrom.Item2);
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
    
    public void UpdateTexture(Component component)
    {
	    TextureRect placedTextureRect = GetTexture(component.beginPosition);
	    bool isNew = placedTextureRect == null;
	    if (isNew)
	    {
		    placedTextureRect = new TextureRect();
		    placedTextureRect.SetPosition(root.ComponentHoverRect.GetGlobalPosition());
		    placedTextureRect.SetRotation(root.ComponentHoverRect.GetParent<ColorRect>().GetRotation());
	    }
	    placedTextureRect.SetTexture(ResourceLoader.Load<Texture2D>(component.GetTexturePath()));
	    if (isNew)
	    {
		    root.AddChild(placedTextureRect);
		    Textures.Add(component.beginPosition, placedTextureRect);
	    }
    }

    public bool CanPlaceComponent(Component component)
    {
	    for (int i = component.beginPosition.X; i < component.endPosition.X; i++)
		{
			for (int j = component.beginPosition.Y; j < component.endPosition.Y; j++)
			{
				if (ComponentExists(new Vector2I(i, j))) return false;
			}
		}
		return true;
    }
}