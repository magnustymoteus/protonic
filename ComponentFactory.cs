namespace protonic;
using Godot;

public class ComponentFactory
{
    
    public Component CreateComponent(string componentName, Vector2 position, Vector2 tileSize)
    {
        switch (componentName)
        {
            case "Beamline Tube":
                return CreateBeamlineTube(position, tileSize);
            default:
                return null;
        }
    }
    public BeamlineTube CreateBeamlineTube(Vector2 position, Vector2 tileSize)
    {
        return new BeamlineTube(position, tileSize);
    }
    
}