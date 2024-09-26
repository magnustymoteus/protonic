namespace protonic.utils;

using Godot;

public class VectorConverter
{
    public static Vector2I Convert(Vector2 vector)
    {
        return new Vector2I((int)vector.X, (int)vector.Y);
    }

    public static Vector2 Convert(Vector2I vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
}

