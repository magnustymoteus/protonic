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
	

    public BeamlineTube GetConnectedTube()
    {
        return (BeamlineTube) occupiedElementConnections.Values.ToArray()[0];
    }
	
    private Path2D CreateBeamPath(Particle particle)
    {
        Curve2D curve = new Curve2D();
        PositionMomentum currentTrajectory = particle.Trajectory;
        curve.AddPoint(currentTrajectory.Position);
        foreach (Element currentElement in GetFilteredConnectedElements<BeamlineTube>())
        {
            if (currentElement is BeamlineTube currentTube)
            {
                currentTrajectory = currentTrajectory.Multiply(currentTube.GetTransferMatrix4x4());
                curve.AddPoint(currentTrajectory.Position);
            }
        }
        Path2D result = new Path2D();
        result.SetCurve(curve);
        return result;
    }


    public bool Emit()
    {
        BeamlineTube tube = GetConnectedTube();
        if (tube != null)
        {
            ConnectiveDirection dir = occupiedConnections.Values.ToArray()[0].ToArray()[0];
            Vector2 targetPos = rectBeginPosition+Matrix2x2.GetRotationMatrix(this.rotation).Multiply(allowedConnections.Keys.ToArray()[0].Item1);
            Electron electron = new Electron(targetPos*32+new Vector2(15,15), Convert(dir));
            Path2D beamPath = CreateBeamPath(electron);
            GetTree().GetRoot().AddChild(beamPath);
            beamPath.AddChild(electron);
        }
        return tube != null;
    }
}