namespace protonic;
using Godot;
public partial class Circle : Node2D
{
    public float radius;
    public Color color;
    public Vector2 circlePosition;
    public override void _Draw()
    {
        DrawCircle(new Vector2(16,16), this.radius, this.color);
    }

    public Circle(Vector2 position, float radius, Color circleColor)
    {
        this.radius = radius;
        this.color = circleColor;
        this.circlePosition = position;
    }

    public override void _Ready()
    {
        SetGlobalPosition(this.circlePosition);
    }
}