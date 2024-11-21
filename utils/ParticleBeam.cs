using Godot;

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

    public float GetEnvelope(float length)
    {
        return Mathf.Sqrt(Emittance * GetBeta(length));
    }
}