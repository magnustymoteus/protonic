namespace protonic.Particle;
using Godot;

public partial class Particle : Node2D
{
    public Vector2 Momentum { get; set; }
    public double Mass { get; set; }  // In kilograms
    public double Charge { get; set; }  // In Coulombs
    public double Energy { get; set; }  // In eV (electron volts)
    
    public Particle(Vector2 initialPosition, Vector2 initialMomentum, double mass, double charge, double energy)
    {
        Position = initialPosition;
        Momentum = initialMomentum;
        Mass = mass;
        Charge = charge;
        Energy = energy;
    }
    public override void _Draw()
    {
        DrawCircle(Position, 5, Colors.Yellow);
    }

    public void UpdatePosition(Vector2 displacement)
    {
        Position += displacement;
    }

    public void ApplyTransferMatrix(Transform2D matrix)
    {
        Vector2 trajectoryVector = new Vector2(Position.X, Momentum.X);
        trajectoryVector = matrix.BasisXform(trajectoryVector);

        Position = new Vector2(trajectoryVector.X, Position.Y);
        Momentum = new Vector2(trajectoryVector.Y, Momentum.Y);
    }
}