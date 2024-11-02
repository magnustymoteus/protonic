using System.Numerics;

namespace protonic.Particle;
using Godot;
using protonic.utils;

public partial class Particle : Node2D
{
    public Godot.Vector2 Momentum { get; set; }
    public double Mass { get; set; }  // In kilograms
    public double Charge { get; set; }  // In Coulombs
    
    public Particle(Vector2 initialPosition, Vector2 initialMomentum, double mass, double charge)
    {
        Position = initialPosition;
        Momentum = initialMomentum;
        Mass = mass;
        Charge = charge;
    }
    public override void _Draw()
    {
        DrawCircle(Position, 5, Colors.Yellow);
        DrawLine(Position, Position+Momentum.Normalized()*50.0f, Colors.Red, 2.5f);
    }

    public void UpdatePosition(double deltaTime)
    {
        //Position += Momentum * (float)deltaTime;
    }

    public void ApplyTransferMatrix(Matrix4x4 matrix)
    {
        GD.Print(matrix);
        System.Numerics.Vector4 trajectoryVector = new System.Numerics.Vector4(Position.X, Momentum.X, Position.Y, Momentum.Y);
        System.Numerics.Vector4 outTrajectoryVector = System.Numerics.Vector4.Transform(trajectoryVector, matrix);
        
        GD.Print("pre:",trajectoryVector);
        GD.Print("position: " , trajectoryVector.X, " ", trajectoryVector.Z);
        GD.Print("momentum: ", trajectoryVector.Y, " ", trajectoryVector.W);
        
        GD.Print("post:",outTrajectoryVector);
        GD.Print("position: " , outTrajectoryVector.X, " " , outTrajectoryVector.Z);
        GD.Print("momentum: ", outTrajectoryVector.Y, " ", outTrajectoryVector.W);
        
        Position = new Vector2(outTrajectoryVector.X, outTrajectoryVector.Z);
        Momentum = new Vector2(outTrajectoryVector.Y, outTrajectoryVector.W);
    }
}

public partial class Proton : Particle
{
    public Proton(Vector2 initialPosition, Vector2 initialMomentum) 
        : base(initialPosition, initialMomentum, 938.27, 1) {}
}

public partial class Neutron : Particle
{
    public Neutron(Vector2 initialPosition, Vector2 initialMomentum)
        : base(initialPosition, initialMomentum, 939.57, 0) {}
}

public partial class Electron : Particle
{
    public Electron(Vector2 initialPosition, Vector2 initialMomentum)
        : base(initialPosition, initialMomentum, 0.511, -1) {}
}

public partial class Positron : Particle
{
    public Positron(Vector2 initialPosition, Vector2 initialMomentum) 
        : base(initialPosition, initialMomentum, 0.511, 1) {}
}