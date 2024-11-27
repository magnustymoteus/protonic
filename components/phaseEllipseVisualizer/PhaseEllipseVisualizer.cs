using Godot;
using Godot.Collections;
using protonic.utils;

public partial class PhaseEllipseVisualizer : Control
{
	private ParticleBeam particleBeam;
	
	private string[] magnets = { "focusing_quadrupole", "drift", "defocusing_quadrupole", "drift" };
	private int magnetIndex;

	private int beamLimit = 500;

	private float xMin = float.MaxValue, xMax = float.MinValue;
	private float yMin = float.MaxValue, yMax = float.MinValue;

	
	private Vector2 screenSize = DisplayServer.ScreenGetSize();

	private Array<Vector2> points = new Array<Vector2>();
	
	public override void _Ready()
	{
		particleBeam = new ParticleBeam(-1.0f, 5.0f, 1.0f);
		PlotPhaseSpaceEllipse();
	}
	

	public override void _Draw()
	{
		DrawLine(new Vector2(xMin, 1920/2.0f), new Vector2(xMax, 1920/2.0f), Colors.White);
		DrawLine(new Vector2(xMin, 1920/2.0f), new Vector2(xMax, 1920/2.0f), Colors.White);
		for (int i = 0; i < points.Count; i++)
		{
			DrawLine(points[i], points[i+1], Colors.Green);
		}
	}

	private void ResizePoints()
	{
		for (int i = 0; i < points.Count; i++)
		{
			points[i] = new Vector2(points[i].X*screenSize.X/2.0f, points[i].Y*screenSize.Y/2.0f);
		}
	}
	private void NormalizePoints()
	{
		for (int i =0;i<points.Count;i++)
		{
			points[i] = new Vector2(points[i].X + Mathf.Abs(xMin), points[i].Y + Mathf.Abs(yMin));
		}
		xMin += Mathf.Abs(xMin);
		xMax += Mathf.Abs(xMin);
		yMin += Mathf.Abs(yMin);
		yMax += Mathf.Abs(yMin);
		
		float xLength = xMax - xMin, yLength = yMax - yMin;
		for (int i =0;i<points.Count;i++)
		{
			points[i] = new Vector2(points[i].X / xLength, points[i].Y / yLength);
		}
	}
	private void UpdateBounds(float x, float y)
	{
		xMin = Mathf.Min(x, xMin);
		xMax = Mathf.Max(x, xMax);
		yMin = Mathf.Min(y, yMin);
		yMax = Mathf.Max(y, yMax);
	}

	private void PlotPhaseSpaceEllipse()
	{
		for (float beamIndex = 0; beamIndex < beamLimit; beamIndex++)
		{
			Vector2 point = particleBeam.GetPhaseSpace(beamIndex, beamLimit);
			points.Add(point);
			magnetIndex = (magnetIndex + 1) % magnets.Length;
			UpdateBounds(point.X, point.Y);
		} 
		NormalizePoints();
		ResizePoints();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
