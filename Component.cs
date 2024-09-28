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

public class Component 
{
    public string path;
    
    // post rotation
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public float rotation;
    
    
    // relative position (pre-rotation) -> direction

    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> availableConnections;
    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> connections;
    
    public static Dictionary<ConnectiveDirection, Vector2I> TransformMap = new Dictionary<ConnectiveDirection, Vector2I>
    {
        { ConnectiveDirection.Left, new Vector2I(-1, 0) },
        { ConnectiveDirection.Up, new Vector2I(0, -1) },
        { ConnectiveDirection.Down, new Vector2I(0, 1) },
        { ConnectiveDirection.Right, new Vector2I(1, 0) }

    };

    public Array<Sprite2D> arrows = new Array<Sprite2D>();
    
    public Component(string path, Vector2I position, Vector2I size, float rotation)
    {
        this.path = path;
        this.beginPosition = GetBeginPosition(position, size);
        this.endPosition = GetEndPosition(position, size);
        this.rotation = rotation;
        
        this.availableConnections = new Dictionary<Vector2I, SortedSet<ConnectiveDirection>>();
        this.connections = new Dictionary<Vector2I, SortedSet<ConnectiveDirection>>();
        
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
            Vector2 position = (connection.Item2-Component.Convert(connection.Item1)/new Vector2(2.0f, 2.0f)) * tileSize;
            Texture2D texture = ResourceLoader.Load<Texture2D>("./images/connectionArrow.png");
            float rotation = ((int)connection.Item1 + 3) % 4 * Mathf.Pi / 2;
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
        return Component.Convert(Matrix2x2.GetRotationMatrix(this.rotation).Transpose()
            .Multiply(Component.Convert(direction)));
    }

    public void AddConnection(Vector2I position, ConnectiveDirection direction)
    {
        SortedSet<ConnectiveDirection> existingConnections;
        this.connections.TryGetValue(position, out existingConnections);
        if (existingConnections == null)
        {
            existingConnections = new SortedSet<ConnectiveDirection>();
            existingConnections.Add(direction);
            connections.Add(position, existingConnections);
        }
        else connections[position].Add(direction);

        availableConnections[position].Remove(UndoRotation(direction));
    }

    public void RemoveConnection(Vector2I position, ConnectiveDirection direction)
    {
        this.connections[position].Remove(direction);
        if (this.connections[position].Count == 0) this.connections.Remove(position);
        availableConnections[position].Add(UndoRotation(direction));
    }
    
    public string GetTexturePath()
    {
        string result = this.path + "/";
        if (connections.Count == 0) result += "disconnected";
        else result += "connected";
        foreach (var connection in connections)
        {
            result += "_" + connection.Key.X + "_" + connection.Key.Y + "_";
            SortedSet<ConnectiveDirection> newConnections = new SortedSet<ConnectiveDirection>();
            foreach (ConnectiveDirection direction in connection.Value)
            {
                newConnections.Add(UndoRotation(direction));
            }

            foreach (ConnectiveDirection newDirection in newConnections)
            {
                result += newDirection.ToString()[0];
            }
        }
        return result + ".png";
    }

    public bool CanConnect(Vector2I position)
    {
        foreach (var target in GetAvailableConnectionTargets())
        {
            if (target.Item2 == position) return true;
        }

        return false;
    }
    public SortedSet<Tuple<ConnectiveDirection, Vector2I>> GetAvailableConnectionTargets()
    {
        SortedSet<Tuple<ConnectiveDirection, Vector2I>> result = new SortedSet<Tuple<ConnectiveDirection, Vector2I>>();
		
        foreach (var availableConnectionFrom in availableConnections)
        {
            foreach (var direction in availableConnectionFrom.Value)
            {
                Matrix2x2 rotationMatrix = Matrix2x2.GetRotationMatrix(this.rotation);
                Vector2I position = GetTargetPosition(availableConnectionFrom.Key, direction);
                ConnectiveDirection rotatedDirection = Component.Convert(rotationMatrix.Multiply(Component.Convert(direction)));
                result.Add(new Tuple<ConnectiveDirection, Vector2I>(rotatedDirection, position));
            }
        }

        return result;
    }

    public Vector2I GetTargetPosition(Vector2I position, ConnectiveDirection direction)
    {
        return this.beginPosition + Matrix2x2.GetRotationMatrix(this.rotation).Multiply(position + Component.Convert(direction));
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
        Variant contents = FileLoader.LoadJsonFromFile(FileLoader.SearchFile(this.path, "connections.json"));
        Dictionary<char, int> directionMapper = new Dictionary<char, int>
            { { 'L', 0 }, { 'U', 1 }, { 'R', 2 }, { 'D', 3 } };
        foreach (var d in contents.AsGodotDictionary())
        {
            string currentDirections = d.Key.ToString();
            SortedSet<ConnectiveDirection> directions = new SortedSet<ConnectiveDirection>();
            foreach (char c in currentDirections)
            {
                ConnectiveDirection direction = (ConnectiveDirection)directionMapper[c];
                directions.Add(direction);
            }
            foreach (Array<int> arr in d.Value.AsGodotArray())
            {
                this.availableConnections.Add(new Vector2I(arr[0], arr[1]), directions);
            }
        }
    }
    
}
public class BeamlineTube : Component
{
    public BeamlineTube(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size, rotation)
    {
    }
}
