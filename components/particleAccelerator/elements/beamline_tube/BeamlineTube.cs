using Godot;
using System;
using System.Linq;
using System.Numerics;
using Godot.Collections;
using protonic;
using protonic.utils.TransferMatrix;

public partial class BeamlineTube : Node2D
{

	public Path2D Path;
	public Array<string> BeamlineType = new Array<string>(); // index : type
	private Gradient _color;
	

	public override void _Ready()
	{
		Path = new Path2D();
		Path.SetCurve(new Curve2D());
		_color = new Gradient();
	}
	
	public void AddPoint(Godot.Vector2 point, string type)
	{
		Path.GetCurve().AddPoint(point);
		QueueRedraw();
		BeamlineType.Add(type);
		_color.AddPoint(32.0f, GetColor(type));
	}
	public override void _Draw()
	{
			Curve2D curve = Path.GetCurve();
			curve.Tessellate();
			DrawPolylineColors(curve.GetBakedPoints(), _color.GetColors(), 20.0f, true);
			for (int i = 0; i < curve.GetPointCount(); i++)
			{
				Godot.Vector2 point = curve.GetPointPosition(i);
				DrawCircle(point, 5.0f, Colors.White);
				DrawLine(point, point + curve.GetPointIn(i), Colors.Red, 2.5f);
				DrawLine(point, point + curve.GetPointOut(i), Colors.Red, 2.5f);
			}
	}

	private Color GetColor(string type)
	{
		string[] pathArr = type.Split("/").Where(x => x != "").ToArray();
		switch (pathArr[1])
		{
			case "bending_magnet":
				return Colors.DarkGreen;
			case "focusing_magnet":
				return Colors.DarkBlue;
			case "defocusing_magnet":
				return Colors.DarkRed;
			case "rf_cavity":
				return Colors.Orange;
			default:
				return Colors.White;
		}
	}

	public Matrix2x2 GetTransferMatrix2x2(string type)
	{
		string[] pathArr = type.Split("/").Where(x => x != "").ToArray();
		switch (pathArr[1])
		{
			case "bending_magnet":
				return TransferMatrix2x2Factory.Dipole(3.2f, 0.2f);
			case "focusing_magnet":
				return TransferMatrix2x2Factory.FocusingQuadrupole(1, 3.2f);
			case "defocusing_magnet":
				return TransferMatrix2x2Factory.DefocusingQuadrupole(-1, 3.2f);
			case "rf_cavity":
				return TransferMatrix2x2Factory.RFCavity(3.2f, 2.0f); // to do
			default:
				return TransferMatrix2x2Factory.Drift(3.2f);
		}
	}

	public Matrix4x4 GetTransferMatrix4x4(string type)
	{
		string[] pathArr = type.Split("/").Where(x => x != "").ToArray();
		switch (pathArr[1])
		{
			case "bending_magnet":
				return TransferMatrix4x4Factory.Dipole(3.2f, 0.2f);
			case "focusing_magnet":
				return TransferMatrix4x4Factory.FocusingQuadrupole(1, 3.2f);
			case "defocusing_magnet":
				return TransferMatrix4x4Factory.DefocusingQuadrupole(-1, 3.2f);
			case "rf_cavity":
				return TransferMatrix4x4Factory.Drift(3.2f); // to do
			default:
				return TransferMatrix4x4Factory.Drift(3.2f);
		}
		
	}
}
