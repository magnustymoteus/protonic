using Godot;
using System;
using System.Linq;
using Godot.Collections;
using protonic;
using protonic.utils;

public partial class ParticleEmitter : Element
{
    public ParticleEmitter(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
        rotation) { }

    public BeamlineTube connectedBeamline;

    public void ConnectBeamline(BeamlineTube beamlineTube)
    {
        connectedBeamline = beamlineTube;
    }
	
    private Path2D CreateBeamPath(Particle particle)
    {
        Curve2D curve = new Curve2D();
        PositionMomentum currentTrajectory = particle.Trajectory;
        curve.AddPoint(currentTrajectory.Position);
        for(int i=1;i<connectedBeamline.Path.GetCurve().GetPointCount();i++)
        {
            currentTrajectory = currentTrajectory.Multiply(connectedBeamline.GetTransferMatrix4x4(connectedBeamline.BeamlineType[i]));
            curve.AddPoint(currentTrajectory.Position);
        }
        Path2D result = new Path2D();
        result.SetCurve(curve);
        return result;
    }
}