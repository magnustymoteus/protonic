using Godot;
using protonic.utils.TransferMatrix;
namespace protonic.utils;

public partial class ParticleBeam : GodotObject
{
    public float Alpha, Beta, Emittance;
    
    public float GetPosition(float length)
    {
        // to do
        return 0;
    }

    public float GetGamma()
    {
        return (1 + Mathf.Pow(Alpha, 2)) / Beta;
    }
    public float GetAlpha(float length)
    {
        return Alpha - length * GetGamma();
    }

    public float GetBeta(float length)
    {
        return Beta - (2 * Alpha * length) + (GetGamma() * Mathf.Pow(length, 2));
    }

    public float GetEnvelope(float length, string magnet)
    {
        Matrix2x2 transferMatrix = new Matrix2x2(0,0,0,0);
            switch (magnet)
            {
                case "defocusing_quadrupole":
                    transferMatrix = TransferMatrix2x2Factory.DefocusingQuadrupole(-1, 3.2f);
                    break;
                case "focusing_quadrupole":
                    transferMatrix = TransferMatrix2x2Factory.FocusingQuadrupole(1, 3.2f);
                    break;
                case "drift":
                    transferMatrix = TransferMatrix2x2Factory.Drift(3.2f);
                    break;
            }

        float C = transferMatrix[0, 0], S = transferMatrix[0,1], Cprime = transferMatrix[1,0], Sprime = transferMatrix[1,1];
        GD.Print("C:", C);
        GD.Print("S:", S);
        float beta = (Mathf.Pow(C, 2) * this.Beta) - (2 * S * C * this.Alpha) + (Mathf.Pow(S, 2) * this.GetGamma());
        GD.Print("Beta:", beta);
        return Mathf.Sqrt(Emittance) * Mathf.Sqrt(beta);
    }
}