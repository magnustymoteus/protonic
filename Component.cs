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
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public float rotation;
    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> availableConnections;
    public Dictionary<Vector2I, SortedSet<ConnectiveDirection>> connections;
    public static Dictionary<ConnectiveDirection, Vector2I> TransformMap = new Dictionary<ConnectiveDirection, Vector2I>
    {
        { ConnectiveDirection.Left, new Vector2I(-1, 0) },
        { ConnectiveDirection.Up, new Vector2I(0, -1) },
        { ConnectiveDirection.Down, new Vector2I(0, 1) },
        { ConnectiveDirection.Right, new Vector2I(1, 0) }

    };
    
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

    public void AddConnection(Vector2I position, ConnectiveDirection direction)
    {
        GD.Print("adding connection ", position, ", ", direction.ToString());

        SortedSet<ConnectiveDirection> existingConnections;
        this.connections.TryGetValue(position, out existingConnections);
        if (existingConnections == null)
        {
            existingConnections = new SortedSet<ConnectiveDirection>();
            existingConnections.Add(direction);
            connections.Add(position, existingConnections);
        }
        else connections[position].Add(direction);
    }
    public string GetTexturePath()
    {
        string result = this.path + "/";
        if (connections.Count == 0) result += "disconnected";
        else result += "connected";
        foreach (var connection in connections)
        {
            result += "_" + connection.Key.X + "_" + connection.Key.Y + "_";
            foreach (ConnectiveDirection direction in connection.Value)
            {
                result += direction.ToString()[0];
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
                Vector2I position = Matrix2x2.GetRotationMatrix(rotation).Multiply(
                    this.beginPosition + availableConnectionFrom.Key + Component.Convert(direction));
                result.Add(new Tuple<ConnectiveDirection, Vector2I>(direction, position));

            }
        }

        return result;
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
