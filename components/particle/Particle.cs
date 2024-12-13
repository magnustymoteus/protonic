using Godot;
using protonic.utils;
using System.Numerics;
using Godot.Collections;
using protonic;

public partial class Particle : PathFollow2D
{
    public PositionMomentum Trajectory;

    public double Mass { get; set; }  // In kilograms
    public double Charge { get; set; }  // In Coulombs

    private bool _interpolating = false;
    private PositionMomentum _trajectoryDest;
    
    public Particle(Godot.Vector2 initialPosition, Godot.Vector2 initialMomentum, double mass, double charge)
    {
        _trajectoryDest = Trajectory = new PositionMomentum(initialPosition, initialMomentum);
        Mass = mass;
        Charge = charge;
    }


    public override void _Draw()
    {
        DrawCircle(new(0,0), 2, Colors.Yellow);
    }
    
    public override void _Ready()
    {
        SetGlobalPosition(Trajectory.Position);
        SetZIndex(50);
    }

    public override void _PhysicsProcess(double delta)
    {
        SetProgress(GetProgress() + 64.0f*(float)delta);
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