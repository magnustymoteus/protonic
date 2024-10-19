namespace protonic.utils;
using Godot;

public class TransferMatrix
{
    public static Transform2D DriftMatrix(float length)
    {
        return new Transform2D(1, length, 0, 0, 1, 0);
    }

    public static Transform2D FocusingQuadrupole(float k, float length)
    {
        float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
        return new Transform2D(Mathf.Cos(omega), Mathf.Sin(omega) / Mathf.Sqrt(Mathf.Abs(k)), 
            -Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sin(omega), Mathf.Cos(omega), 0, 0);
    }

    public static Transform2D DefocusingQuadrupole(float k, float length)
    {
        float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
        return new Transform2D(Mathf.Cosh(omega), Mathf.Sinh(omega) / Mathf.Sqrt(Mathf.Abs(k)),
            Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sinh(omega), Mathf.Cosh(omega), 0, 0);
    }

    public static Transform2D DipoleMatrix(float radius, float angle)
    {
        return new Transform2D(Mathf.Cos(angle), radius * Mathf.Sin(angle), 
            -Mathf.Sin(angle) / radius, Mathf.Cos(angle), 0, 0);
    }
}
