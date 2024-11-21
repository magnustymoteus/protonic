using Godot;
public class Matrix2x2
{
    private readonly float[,] _matrix;

    public float this[int x, int y]
    {
        get
        {
            return _matrix[x, y];
        } 
          
        // using set accessor 
        set
        { 
            _matrix[x,y] = value; 
        } 
    }

    public Matrix2x2(float m11, float m12, float m21, float m22)
    {
        _matrix = new float[,]
        {
            { m11, m12 },
            { m21, m22 }
        };
    }
    
    public static Matrix2x2 GetRotationMatrix(float angle)
    {
        return new Matrix2x2(
            (int)Mathf.Cos(angle), (int)-Mathf.Sin(angle),
            (int)Mathf.Sin(angle),  (int)Mathf.Cos(angle)
        );
    }
    public Matrix2x2(double m11, double m12, double m21, double m22) : this((float) m11, (float) m12, (float) m21, (float) m22) {}
    public Vector2 Multiply(Vector2 vector)
    {
        float x = _matrix[0, 0] * vector.X + _matrix[0, 1] * vector.Y;
        float y = _matrix[1, 0] * vector.X + _matrix[1, 1] * vector.Y;
        return new Vector2(x, y);
    }

    public Vector2I Multiply(Vector2I vector)
    {
        Vector2 result = Multiply(new Vector2(vector.X, vector.Y));
        return new Vector2I((int)result.X, (int)result.Y);
    }
    public Matrix2x2 Transpose()
    {
        return new Matrix2x2(
            _matrix[0, 0], _matrix[1, 0],
            _matrix[0, 1], _matrix[1, 1]
        );
    }
}