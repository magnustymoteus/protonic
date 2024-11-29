using Godot;

namespace protonic;

public class ElementFactory
{
    public Element CreateElement(string name, Vector2I position, Vector2I size, float rotation)
    {
        switch (name.Replace("/", ""))
        {
            case "control_console":
                return new ControlConsole(name, position, size, rotation);
            default:
                return new Element(name, position, size, rotation);
        }
    }
}