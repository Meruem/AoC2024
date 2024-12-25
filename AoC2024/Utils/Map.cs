namespace AoC2024.Utils;

public class Grid(int width, int height)
{
    private readonly char[] _grid = new char[width * height];
    public int Height => height;
    public int Width => width;

    public static Grid FromLines(string[] lines)
    {
        var height = lines.Length;
        var width = lines[0].Length;
        var grid = new Grid(width, height);
        var i = 0;
        foreach (var line in lines)
        foreach (var ch in line)
        {
            grid._grid[i] = ch;
            i++;
        }
        return grid;
    }
    
    private int Index(Vector2 pos) => pos.Y * height + pos.X;
    
    public bool HasElementAt(char element, Vector2 position)
        => IsInRange(position) && _grid[Index(position)] == element;

    public bool IsInRange(Vector2 position) =>
        position.Y >= 0 && position.Y < height && position.X >= 0 && position.X < width;
    
    public Vector2? FindPosition(char element)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_grid[y * height + x] == element) return new Vector2(x, y);
            }
        }

        return null;
    }


}

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

    public static void SetElementAt(this IList<string> map, char element, Vector2 position)
    {
        var s = map[position.Y];
        s = s.Remove(position.X, 1);
        s = s.Insert(position.X, element.ToString());
        map[position.Y] = s;
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