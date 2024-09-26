namespace protonic;
using Godot;

public class ComponentFactory
{
    
    public Component CreateComponent(string path, string componentName, Vector2I position, Vector2I size, float rotation)
    {
        switch (componentName)
        {
            case "beamline_tube":
                return CreateBeamlineTube(path,position, size, rotation);
            default:
                return null;
        }
    }
    public BeamlineTube CreateBeamlineTube(string path, Vector2I position, Vector2I size, float rotation)
    {
        return new BeamlineTube(path, position, size, rotation);
    }
    
}