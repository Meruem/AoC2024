namespace AoC2024;

public static class Map
{
    public static bool HasElementAt(this IList<string> list, char element, Vector2 position)
        => IsInRange(list, position) && list[position.Y][position.X] == element;

    public static bool IsInRange(this IList<string> list, Vector2 position) =>
        position.Y >= 0 && position.Y < list.Count && position.X >= 0 && position.X < list[position.Y].Length;

    public static Vector2? FindPosition(this IList<string> lines, char element)
    {
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == element) return new Vector2(x, y);
            }
        }

        return null;
    }

    public static IEnumerable<Vector2> Positions(this IList<string> lines)
    {
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                yield return new Vector2(x, y);
            }
        }
    }
    
    public static IEnumerable<Tuple<char ,Vector2>> ElementsWithPos(this IList<string> lines)
    {
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                yield return new (lines[y][x], new Vector2(x, y));
            }
        }
    }

    public static void ForEachWithPos(this IList<string> lines, Action<char, Vector2> action)
    {
        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                action(lines[y][x], new Vector2(x, y));
            }
        }
    }

    public static char GetCharAt(this IList<string> lines, Vector2 position) => lines[position.Y][position.X];
}