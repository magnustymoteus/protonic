using System.Linq;
using Godot;

namespace protonic;

public class ElementFactory
{
    public Element CreateElement(string path, Vector2I position, Vector2I size, float rotation)
    {
        string[] pathArr = path.Split("/").Where(x => x != "").ToArray();
        switch (pathArr[0])
        {
            case "control_console":
                return new ControlConsole(path, position, size, rotation);
            case "beamline_tube":
                return new BeamlineTube(path, position, size, rotation);
            default:
                return new Element(path, position, size, rotation);
        }
    }
}