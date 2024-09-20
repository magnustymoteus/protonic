namespace protonic;
using Godot;

public class ComponentFactory
{
    
    public Component CreateComponent(string componentName, Vector2I position, Vector2I size, float rotation)
    {
        switch (componentName)
        {
            case "beamline_tube":
                return CreateBeamlineTube(position, size, rotation);
            default:
                return null;
        }
    }
    public BeamlineTube CreateBeamlineTube(Vector2I position, Vector2I size, float rotation)
    {
        return new BeamlineTube(position, size, rotation);
    }
    
}