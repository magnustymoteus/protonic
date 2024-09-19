namespace protonic;
using Godot;

public class Component
{
    protected string name;
    public Vector2I position;
    

    public Component(string name, Vector2I position)
    {
        this.name = name;
        this.position = position;
    }

    public Component(string name, Vector2 position, Vector2 tileSize)
    {
        this.name = name;
        this.position = new Vector2I(Mathf.FloorToInt(position.X/tileSize.X)-1, Mathf.FloorToInt(position.Y/tileSize.Y)-1);
    }
}

public class BeamlineTube : Component
{
    public BeamlineTube(Vector2 position, Vector2 tileSize) : base("Beamline Tube", position, tileSize)
    {
    }
}