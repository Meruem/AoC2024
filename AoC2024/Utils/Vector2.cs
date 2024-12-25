namespace AoC2024.Utils;

public readonly record struct Vector2(int X, int Y)
{
    public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 a, int x) => new Vector2(a.X * x, a.Y * x);
    public Vector2 RotateClockwise() => new Vector2(-Y, X);
    public Vector2 RotateCounterClockwise() => new Vector2(Y, -X);
    public int Distance => Math.Abs(X) + Math.Abs(Y);
}