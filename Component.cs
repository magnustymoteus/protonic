using System;
using protonic.utils;

namespace protonic;
using Godot;
using System.Collections.Generic;

public enum ConnectiveDirection
{
    Left=0, Up=1, Right=2, Down=3
}

public 

public class Component
{
    public string name;
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public float rotation;
    public Dictionary<ConnectiveDirection, Component> connections;
    public SortedSet<ConnectiveDirection> possibleDirections;

    public Component(string name, Vector2I position, Vector2I size, float rotation)
    {
        this.name = name;
        this.beginPosition = position;
        this.endPosition = position + size;
        // DO THIS
        this.rotation = rotation;
        this.connections = new Dictionary<ConnectiveDirection, Component>();
        this.possibleDirections = new SortedSet<ConnectiveDirection>();
    }
    

    public static SortedSet<ConnectiveDirection> GetDirections(string filePath)
    {
        string contents = FileLoader.SearchFile(filePath, "connections.txt");
        SortedSet<ConnectiveDirection> directions = new SortedSet<ConnectiveDirection>();
        foreach (var c in contents)
        {
            ConnectiveDirection direction = Enum.Parse<ConnectiveDirection>(c.ToString());
            directions.Add(direction);
        }
        return directions;
    }

    public void ImportPossibleDirections(string filePath)
    {
        this.possibleDirections = Component.GetDirections(filePath);
    }
}

public class BeamlineTube : Component
{
    public BeamlineTube(Vector2I position, Vector2I size, float rotation) : base("Beamline Tube", position, size, rotation)
    {
    }
}