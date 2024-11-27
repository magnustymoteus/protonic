using Godot;
using protonic.utils.TransferMatrix;
using System;
using protonic.utils.Particle;
namespace protonic.utils;

public partial class ParticleBeam : GodotObject
{
    public float Alpha, Beta, Emittance;

    private Random _random;

    public float GetGamma()
    {
        return (1 + Mathf.Pow(Alpha, 2)) / Beta;
    }

    public ParticleBeam()
    {
        _random = new Random();
    }

    public Matrix2x2 GetTransferMatrix(string magnet, float length = 3.2f)
    {
        Matrix2x2 transferMatrix = new Matrix2x2(0, 0, 0, 0);
        switch (magnet)
        {
            case "defocusing_quadrupole":
                transferMatrix = TransferMatrix2x2Factory.DefocusingQuadrupole(-1, length);
                break;
            case "focusing_quadrupole":
                transferMatrix = TransferMatrix2x2Factory.FocusingQuadrupole(1, length);
                break;
            case "drift":
                transferMatrix = TransferMatrix2x2Factory.Drift(length);
                break;
        }

        return transferMatrix;
    }

    public Godot.Vector2 GetPhaseSpace(float s, string magnet, float sMax)
    {
        Matrix2x2 transferMatrix = GetTransferMatrix(magnet);

        float C = transferMatrix[0, 0],
            S = transferMatrix[0, 1],
            Cprime = transferMatrix[1, 0],
            Sprime = transferMatrix[1, 1];
        float beta = (Mathf.Pow(C, 2) * this.Beta) - (2 * S * C * this.Alpha) + (Mathf.Pow(S, 2) * this.GetGamma());


        float theta = s * 2.0f*Mathf.Pi / sMax;
        float xPrime = -Mathf.Sqrt(Emittance / beta) * (Alpha * Mathf.Cos(theta) + Mathf.Sin(theta));
        float x = Mathf.Sqrt(Emittance*beta)*Mathf.Cos(theta);
        return new Godot.Vector2(x, xPrime);
    }

    public float GetEnvelope(string magnet)
    {
        Matrix2x2 transferMatrix = GetTransferMatrix(magnet);

        float C = transferMatrix[0, 0],
            S = transferMatrix[0, 1],
            Cprime = transferMatrix[1, 0],
            Sprime = transferMatrix[1, 1];
        float beta = (Mathf.Pow(C, 2) * this.Beta) - (2 * S * C * this.Alpha) + (Mathf.Pow(S, 2) * this.GetGamma());
        return Mathf.Sqrt(Emittance) * Mathf.Sqrt(beta);
    }

    public float GetRandomFactor()
    {  return (float)(_random.NextDouble() * 2 - 1);
    }
}
    