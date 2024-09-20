using System;

namespace protonic;
using Godot;

public class Component
{
    public string name;
    public Vector2I beginPosition;
    public Vector2I endPosition;
    public float rotation;

    public Component(string name, Vector2I position, Vector2I size, float rotation)
    {
        this.name = name;
        this.beginPosition = position;
        this.endPosition = position + size;
        this.rotation = rotation;
    }
}

public class BeamlineTube : Component
{
    public BeamlineTube(Vector2I position, Vector2I size, float rotation) : base("Beamline Tube", position, size, rotation)
    {
    }
}