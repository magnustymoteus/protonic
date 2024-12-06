using Godot;
using System;
using System.Linq;
using System.Numerics;
using protonic;
using protonic.utils.TransferMatrix;

public partial class BeamlineTube : Element
{
	public BeamlineTube(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }

	public Matrix2x2 GetTransferMatrix2x2()
	{
		string[] pathArr = name.Split("/").Where(x => x != "").ToArray();
		switch (pathArr[1])
		{
			case "bending_magnet":
				return TransferMatrix2x2Factory.Dipole(3.2f, 0.2f);
			case "focusing_magnet":
				return TransferMatrix2x2Factory.FocusingQuadrupole(1, 3.2f);
			case "defocusing_magnet":
				return TransferMatrix2x2Factory.DefocusingQuadrupole(-1, 3.2f);
			case "rf_cavity":
				return TransferMatrix2x2Factory.Drift(3.2f); // to do
			default:
				return TransferMatrix2x2Factory.Drift(3.2f);
		}
	}

	public Matrix4x4 GetTransferMatrix4x4()
	{
		string[] pathArr = name.Split("/").Where(x => x != "").ToArray();
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
