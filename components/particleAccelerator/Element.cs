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
    
    
    public Element(string name, Vector2I position, Vector2I size, float rotation)
    {
        this.name = name;
        this.path = "./components/particleAccelerator/elements" + name;
        this.beginPosition = GetBeginPosition(position, size);
        this.endPosition = GetEndPosition(position, size);
        this.rectBeginPosition = position;
        this.rectEndPosition = position + size;
        this.rotation = rotation;
    }
    

    public Vector2I UndoRotation(Vector2I position)
    {
        return Matrix2x2.GetRotationMatrix(this.rotation).Transpose().Multiply(position);
    }
    
    
    public string GetTexturePath()
    {
        return this.path + "/texture.png";
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
    
    
    public override void _UnhandledInput(InputEvent @event)
    {
        
    }
}
