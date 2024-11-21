using Godot;
namespace protonic.utils;
public class PositionMomentum
{
    public Vector2 Position;
    public Vector2 Momentum;

    public PositionMomentum(Vector2 position, Vector2 momentum)
    {
        Position = position;
        Momentum = momentum;
    }
}