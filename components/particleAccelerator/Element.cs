using System;
using System.Linq;
using Godot.Collections;
using protonic.utils;

namespace protonic;
using Godot;
using System.Collections.Generic;
public enum ConnectiveDirection
{
    Left=0, Up=1, Right=2, Down=3
}

public partial class Element : Sprite2D
{
    public string name, path;
    
    // post rotation
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public Vector2I rectBeginPosition;
    public Vector2I rectEndPosition;
    public float rotation;
    
    
    // relative position (pre-rotation) -> direction

    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> availableConnections;
    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> occupiedConnections;
    public Dictionary<Tuple<Vector2I, ConnectiveDirection>, SortedSet<string>> allowedConnections; // (pos, connectiveDirection) -> {elementPaths} 
    
    public Dictionary<Vector2I, Element> occupiedElementConnections; // targetPos -> Element
    
    public static Dictionary<ConnectiveDirection, Vector2I> TransformMap = new Dictionary<ConnectiveDirection, Vector2I>
    {
        { ConnectiveDirection.Left, new Vector2I(-1, 0) },
        { ConnectiveDirection.Up, new Vector2I(0, -1) },
        { ConnectiveDirection.Down, new Vector2I(0, 1) },
        { ConnectiveDirection.Right, new Vector2I(1, 0) }
    };

    public Array<Sprite2D> arrows = new Array<Sprite2D>();
    
    public Element(string name, Vector2I position, Vector2I size, float rotation)
    {
        this.name = name;
        this.path = "./components/particleAccelerator/elements" + name;
        this.beginPosition = GetBeginPosition(position, size);
        this.endPosition = GetEndPosition(position, size);
        this.rectBeginPosition = position;
        this.rectEndPosition = position + size;
        this.rotation = rotation;
        
        // local (relative to rectbeginpos)
        this.availableConnections = new Dictionary<Vector2I, SortedSet<ConnectiveDirection>>();
        this.occupiedConnections = new Dictionary<Vector2I, SortedSet<ConnectiveDirection>>();
        this.allowedConnections = new Dictionary<Tuple<Vector2I, ConnectiveDirection>, SortedSet<string>>();
        this.occupiedElementConnections = new Dictionary<Vector2I, Element>();
        
        ImportPossibleConnections();
    }
    

    public void ClearConnectionArrows(Node2D parent)
    {
        foreach (var arrow in this.arrows)
        {
            parent.RemoveChild(arrow);
        }

        this.arrows.Clear();
    }

    public void PlaceConnectionArrows(Vector2 tileSize, Node2D parent)
    {
        foreach (var connection in this.GetAvailableConnectionTargets())
        {
            Vector2 position = (connection.Item2-Element.Convert(connection.Item1)/new Vector2(2.0f, 2.0f)) * tileSize;
            Texture2D texture = ResourceLoader.Load<Texture2D>("./assets/images/connectionArrow.png");
            float rotation = ((int)connection.Item1+3) % 4 * Mathf.Pi / 2;
            Sprite2D arrowRect = new Sprite2D();
            arrowRect.SetZIndex(2);
            arrowRect.SetTexture(texture);
            arrowRect.SetRotation(rotation);
            arrowRect.SetGlobalPosition(position+tileSize/2);
            arrowRect.SetModulate(new Color(0.0f, 1.0f, 0.0f, 0.8f));
            this.arrows.Add(arrowRect);
            parent.AddChild(arrowRect);
        }
    }
    
    public ConnectiveDirection UndoRotation(ConnectiveDirection direction)
    {
        return Element.Convert(Matrix2x2.GetRotationMatrix(this.rotation).Transpose()
            .Multiply(Element.Convert(direction)));
    }

    public Vector2I UndoRotation(Vector2I position)
    {
        return Matrix2x2.GetRotationMatrix(this.rotation).Transpose().Multiply(position);
    }

    public void AddConnection(Vector2I targetPos, ConnectiveDirection direction)
    {
        SortedSet<ConnectiveDirection> existingConnections;
        this.occupiedConnections.TryGetValue(targetPos, out existingConnections);
        if (existingConnections == null)
        {
            existingConnections = new SortedSet<ConnectiveDirection>();
            existingConnections.Add(direction);
            occupiedConnections.Add(targetPos, existingConnections);
        }
        else occupiedConnections[targetPos].Add(direction);
        
        availableConnections[targetPos].Remove(UndoRotation(direction));
    }

    public void AddElementConnection(Vector2I targetPos, Element element)
    {
        occupiedElementConnections.Add(targetPos, element);
    }

    public void RemoveConnection(Vector2I position, ConnectiveDirection direction)
    {
        this.occupiedConnections[position].Remove(direction);
        if (this.occupiedConnections[position].Count == 0) this.occupiedConnections.Remove(position);
        availableConnections[position].Add(UndoRotation(direction));
        occupiedElementConnections.Remove(GetTargetPosition(position, UndoRotation(direction)));
    }
    
    public string GetTexturePath()
    {
        return this.path + "/texture.png";
    }
    
    // doesn't check element type
    public bool CanConnect(Vector2I position)
    {
        foreach (var target in GetAvailableConnectionTargets())
        {
            if (target.Item2 == position) return true;
        }
        return false;
    }

    // checks element type
    public bool CanConnect(Vector2I position, string elementPath)
    {
        foreach (var target in GetAvailableConnectionTargets())
        {
            if (target.Item2 == position)
            {
                SortedSet<string> allowedElements;
                allowedConnections.TryGetValue(new Tuple<Vector2I, ConnectiveDirection>(ConvertTargetToConnection(target.Item2, target.Item1), UndoRotation(target.Item1)),
                    out allowedElements);
                return allowedElements != null && allowedElements.Any(elementPath.Contains);
            }
        }
        return false;
    }
    public Vector2I ConvertTargetToConnection(Vector2I targetPos, ConnectiveDirection direction)
    {
        return UndoRotation(targetPos-rectBeginPosition)-UndoRotation(Element.Convert(direction)); 
    }
    // convert local target positions to global
    public SortedSet<Tuple<ConnectiveDirection, Vector2I>> GetAvailableConnectionTargets()
    {
        SortedSet<Tuple<ConnectiveDirection, Vector2I>> result = new SortedSet<Tuple<ConnectiveDirection, Vector2I>>();
		
        foreach (var availableConnectionFrom in availableConnections)
        {
            foreach (var direction in availableConnectionFrom.Value)
            {
                Matrix2x2 rotationMatrix = Matrix2x2.GetRotationMatrix(this.rotation);
                Vector2I position = GetTargetPosition(availableConnectionFrom.Key, direction);
                ConnectiveDirection rotatedDirection = Element.Convert(rotationMatrix.Multiply(Element.Convert(direction)));
                result.Add(new Tuple<ConnectiveDirection, Vector2I>(rotatedDirection, position));
            }
        }
        return result;
    }
    public Vector2I GetTargetPosition(Vector2I position, ConnectiveDirection direction)
    {
        return this.rectBeginPosition + Matrix2x2.GetRotationMatrix(this.rotation).Multiply( position + Element.Convert(direction));
    }
    public static Vector2I Convert(ConnectiveDirection direction)
    {
        return TransformMap[direction];
    }

    public static ConnectiveDirection Convert(Vector2I direction)
    {
        return TransformMap.ToDictionary(x => x.Value, x => x.Key)[direction];
    }
    public static Vector2I GetBeginPosition(Vector2I position, Vector2I size)
    {
        Vector2I beginPosition;
        if (position.X < (position + size).X) beginPosition.X = position.X;
        else beginPosition.X = (position + size).X + 1;

        if (position.Y < (position + size).Y) beginPosition.Y = position.Y;
        else beginPosition.Y = (position + size).Y + 1;

        return beginPosition;
    }
    public static Vector2I GetEndPosition(Vector2I position, Vector2I size)
    {
        Vector2I endPosition;
        if (position.X > (position + size).X) endPosition.X = position.X + 1;
        else endPosition.X = (position + size).X;

        if (position.Y > (position + size).Y) endPosition.Y = position.Y + 1;
        else endPosition.Y = (position + size).Y;

        return endPosition;
    }

    public void ImportPossibleConnections()
    {
        Variant contents = FileManager.LoadJsonFromFile(FileManager.SearchFile(this.path, "connections.json"))
            .AsGodotDictionary()["connections"];
        Dictionary<char, int> directionMapper = new Dictionary<char, int>
            { { 'L', 0 }, { 'U', 1 }, { 'R', 2 }, { 'D', 3 } };
        foreach (Dictionary connection in contents.AsGodotArray())
        {
            string currentDirections = connection["connection"].AsString();
            SortedSet<ConnectiveDirection> directions = new SortedSet<ConnectiveDirection>();
            foreach (char c in currentDirections)
            {
                ConnectiveDirection direction = (ConnectiveDirection)directionMapper[c];
                directions.Add(direction);
            }
            foreach (Array<int> arr in connection["positions"].AsGodotArray())
            {
                Vector2I pos = new Vector2I(arr[0], arr[1]);
                this.availableConnections.TryGetValue(pos, out var dirs);
                if (dirs != null) availableConnections[pos].UnionWith(directions);
                else availableConnections.Add(pos, directions);
                foreach (var direction in directions)
                {
                    this.allowedConnections.Add(new Tuple<Vector2I, ConnectiveDirection>(new Vector2I(arr[0], arr[1]), direction), 
                        new SortedSet<string>(connection["allowed"].AsGodotArray<string>()));
                }
            }
        }
    }
    
    public Array<Element> GetFilteredConnectedElements<TElement>(bool getOnlyConsecutive = false)
    {
        Array<Element> elementArr = new Array<Element>();
        Element currentElem = this;
        elementArr.Add(currentElem);
        while(currentElem.occupiedElementConnections.Count > 0 && (!getOnlyConsecutive || currentElem is TElement))
        {
            bool foundCandidate = false;
            foreach (var candidateElem in currentElem.occupiedElementConnections.Values.ToArray())
            {
                if (!elementArr.Contains(candidateElem))
                {
                    currentElem = candidateElem;
                    foundCandidate = true;
                    break;
                }
            }

            if (!foundCandidate) break;
            elementArr.Add(currentElem);
        }
        var result = new Array<Element>(elementArr.Where(element => element is TElement).ToArray());
        return result;
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        
    }
}
