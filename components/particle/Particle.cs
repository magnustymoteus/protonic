using System.Numerics;

using Godot;
using protonic.utils;

public partial class Particle : Node2D
{
    public PositionMomentum Trajectory;
    public double Mass { get; set; }  // In kilograms
    public double Charge { get; set; }  // In Coulombs
    
    
    
    public Particle(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum, double mass, double charge)
    {
        Trajectory = new PositionMomentum(initialPosition, initialMomentum);
        Mass = mass;
        Charge = charge;
    }
    public override void _Draw()
    {
        DrawCircle(Position, 2, Colors.Yellow);
    }
    

    public void ApplyTransferMatrix(Matrix4x4 matrix)
    {
        Matrix4x4 trajectoryMatrix = new Matrix4x4(
            Trajectory.Position.X, 0,0,0,
            Trajectory.Momentum.X, 1,0,0,
            Trajectory.Position.Y, 0,1,0,
            Trajectory.Momentum.Y, 0,0,1
            );
        
        Matrix4x4 outTrajectoryMatrix = Matrix4x4.Multiply(matrix, trajectoryMatrix);
        GD.Print("before:");
        GD.Print(trajectoryMatrix.M11+","+trajectoryMatrix.M21+","+trajectoryMatrix.M31+","+trajectoryMatrix.M41);
        GD.Print("after:");
        GD.Print(outTrajectoryMatrix.M11+","+outTrajectoryMatrix.M21+","+outTrajectoryMatrix.M31+","+outTrajectoryMatrix.M41);
        
        Trajectory.Momentum = new Godot.Vector2(outTrajectoryMatrix.M21, outTrajectoryMatrix.M41);
        Trajectory.Position = new Godot.Vector2(outTrajectoryMatrix.M11, outTrajectoryMatrix.M31);
        SetPosition(new Godot.Vector2(Trajectory.Position.X, Trajectory.Momentum.X));
        QueueRedraw();
    }
}

public partial class Proton : Particle
{
    public Proton(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum) 
        : base(initialPosition, initialMomentum, 938.27, 1) {}
}

public partial class Neutron : Particle
{
    public Neutron(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum)
        : base(initialPosition, initialMomentum, 939.57, 0) {}
}

public partial class Electron : Particle
{
    public Electron(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum)
        : base(initialPosition, initialMomentum, 0.511, -1) {}
}

public partial class Positron : Particle
{
    public Positron(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum) 
        : base(initialPosition, initialMomentum, 0.511, 1) {}
}