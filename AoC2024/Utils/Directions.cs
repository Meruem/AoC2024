namespace AoC2024.Utils;

public static class Directions
{
    public static Vector2 Left => new Vector2(-1, 0);
    public static Vector2 Right => new Vector2(1, 0);
    public static Vector2 Up => new Vector2(0, -1);
    public static Vector2 Down => new Vector2(0, 1);

    public static readonly List<Vector2> AllDirections = [Left, Right, Up, Down];

    public static readonly Dictionary<char, Vector2> DirectionMap = new()
    {
        { '<', Left },
        { '>', Right },
        { '^', Up },
        { 'v', Down },
    };
}