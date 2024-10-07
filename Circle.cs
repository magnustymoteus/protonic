namespace protonic;
using Godot;
public partial class Circle : Node2D
{
    public float radius;
    public Color color;
    public Vector2 circlePosition;
    public override void _Draw()
    {
        DrawCircle(new Vector2(0,0), this.radius, this.color);
    }

    public Circle(Vector2 position, float radius, Color circleColor)
    {
        this.radius = radius;
        this.color = circleColor;
        SetGlobalPosition(position);
    }
}