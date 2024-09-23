using System;

namespace protonic;
using Godot;
using System.Collections.Generic;

public enum ConnectiveDirection
{
    Left, Up, Right, Down
}

public class Component
{
    public string name;
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public float rotation;
    public Dictionary<ConnectiveDirection, Component> connections;

    public Component(string name, Vector2I position, Vector2I size, float rotation)
    {
        this.name = name;
        this.beginPosition = position;
        this.endPosition = position + size;
        this.rotation = rotation;
        this.connections = new Dictionary<ConnectiveDirection, Component>();
    }
}

public class BeamlineTube : Component
{
    public BeamlineTube(Vector2I position, Vector2I size, float rotation) : base("Beamline Tube", position, size, rotation)
    {
    }
}