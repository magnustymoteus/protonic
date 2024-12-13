using System.Numerics;
using Vector2 = Godot.Vector2;

namespace protonic.utils;
public class PositionMomentum
{
    public Vector2 Position;
    public Vector2 Momentum;

    public PositionMomentum(Vector2 position, Vector2 momentum)
    {
        Position = position;
        Momentum = momentum;
    }

    public PositionMomentum Multiply(Matrix4x4 matrix)
    {
        Matrix4x4 trajectoryMatrix = new Matrix4x4(
            Position.X, 0,0,0,
            Momentum.X, 1,0,0,
            Position.Y, 0,1,0,
            Momentum.Y, 0,0,1
        );
        
        Matrix4x4 outTrajectoryMatrix = Matrix4x4.Multiply(matrix, trajectoryMatrix);
        return new PositionMomentum(new Vector2(outTrajectoryMatrix.M11, outTrajectoryMatrix.M31),
            new Vector2(outTrajectoryMatrix.M21, outTrajectoryMatrix.M41));
    }

    public PositionMomentum Multiply(Matrix2x2 matrix, string plane = "x")
    {
        Godot.Vector2 trajectoryVectorX = new Godot.Vector2(Position.X, Momentum.X),
            trajectoryVectorY = new Godot.Vector2(Position.Y, Momentum.Y);
        switch (plane)
        {
            case "y":
                trajectoryVectorY = matrix.Multiply(trajectoryVectorY);
                break;
            default:
                trajectoryVectorX = matrix.Multiply(trajectoryVectorX);
                break;
        }
        return new PositionMomentum(new Godot.Vector2(trajectoryVectorX.X, trajectoryVectorY.X), new Godot.Vector2(trajectoryVectorX.Y, trajectoryVectorY.Y));
    }
}