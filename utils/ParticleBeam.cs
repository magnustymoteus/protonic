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

    public ParticleBeam(float alpha, float beta, float emittance) : this()
    {
        Alpha = alpha;
        Beta = beta;
        Emittance = emittance;
    }
    

    public Vector2 GetPhaseSpace(float s, float sMax)
    {
        float theta = s * 2.0f *Mathf.Pi / sMax;
        float xPrime = -Mathf.Sqrt(Emittance / Beta) * (Alpha * Mathf.Cos(theta) + Mathf.Sin(theta));
        float x = Mathf.Sqrt(Emittance*Beta)*Mathf.Cos(theta);
        return new Vector2(x, xPrime);
    }

    public float GetEnvelope(BeamlineTube tube)
    {
        Matrix2x2 transferMatrix = tube.GetTransferMatrix2x2();

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
    