using Godot;
using System;

public partial class Wire : Node2D
{
	public Path2D Path;

	public override void _Ready()
	{
		Path = new Path2D();
		Path.SetCurve(new Curve2D());
	}
	
	public void AddPoint(Vector2 point)
	{
		Path.GetCurve().AddPoint(point);
		QueueRedraw();
	}
	public override void _Draw()
	{
		Curve2D curve = Path.GetCurve();
		curve.Tessellate();
		DrawPolyline(curve.GetBakedPoints(), Colors.Black, 5.0f, true);
		for (int i = 0; i < curve.GetPointCount(); i++)
		{
			Vector2 point = curve.GetPointPosition(i);
			DrawCircle(point, 5.0f, Colors.White);
			DrawLine(point, point + curve.GetPointIn(i), Colors.Red, 2.5f);
			DrawLine(point, point + curve.GetPointOut(i), Colors.Red, 2.5f);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
