using Godot;
using System;
using Godot.Collections;

public partial class Map : ColorRect
{
	[Export] private World world;

	[Export] private ColorRect followRect;

	public override void _Ready()
	{
		_paths.Add(new Path2D());
		_paths[_currentPathIndex].SetCurve(new Curve2D());
	}

	public override void _Process(double delta)
	{
		
	}
	/* drawing section */
	private Array<Path2D> _paths = new Array<Path2D>();
	private int _currentPathIndex = 0;
	
	public override void _Draw()
	{
		foreach (var path in _paths)
		{
			Curve2D curve = path.GetCurve();
			curve.Tessellate();
			DrawPolyline(curve.GetBakedPoints(), Colors.LightBlue, 20.0f, true);
			for (int i = 0; i < curve.GetPointCount(); i++)
			{
				Vector2 point = curve.GetPointPosition(i);
				DrawCircle(point, 5.0f, Colors.White);
				DrawLine(point, point + curve.GetPointIn(i), Colors.Red, 2.5f);
				DrawLine(point, point + curve.GetPointOut(i), Colors.Red, 2.5f);
			}
		}
	}
	
	public void AddPoint()
	{
		Vector2 pos = GetGlobalMousePosition() / 32.0f;
		pos = new Vector2(Mathf.FloorToInt(pos.X)*32.0f, Mathf.FloorToInt(pos.Y)*32.0f+16.0f);
		_paths[_currentPathIndex].GetCurve().AddPoint(pos);
		QueueRedraw();
	}

	public void ResetDraw()
	{
		_currentPathIndex++;
		_paths.Add(new Path2D());
		_paths[_currentPathIndex].SetCurve(new Curve2D());
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("place") && Input.IsActionPressed("draw"))
		{
			AddPoint();
		}
		else if (Input.IsActionPressed("place"))
		{
			world.ClickedOnMap();
		}
		else if (Input.IsActionPressed("delete"))
		{
			world.DeleteElement();
		}
		else if (Input.IsActionJustReleased("draw"))
		{
			ResetDraw();
		}
	}
}
