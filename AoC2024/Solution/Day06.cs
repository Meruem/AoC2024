namespace AoC2024.Solution;

public static class Day06
{
    public static int Part1()
    {
        var lines = File.ReadAllLines("Input/day06.txt");
        var (current, dir) = GetStartPositionAndDir(lines);
        return NavigateThrough(lines, current, dir)!.Value;
    }

    public static int Part2()
    {
        var lines = File.ReadAllLines("Input/day06.txt");
        var (start, dir) = GetStartPositionAndDir(lines);
        return lines.Positions() // brute force ftw
            .Where(pos => pos != start)
            .Sum(wall => NavigateThrough(lines, start, dir, wall) is null ? 1 : 0);
    }

    private static (Vector2 current, Vector2 dir) GetStartPositionAndDir(string[] lines)
    {
        Vector2 current;
        Vector2 dir;
        var up = lines.FindPosition('^');
        var down = lines.FindPosition('v');
        var left = lines.FindPosition('<');
        var right = lines.FindPosition('>');
        if (up is not null)
        {
            current = up;
            dir = new Vector2(0, -1);
        }
        else if (down is not null)
        {
            current = down;
            dir = new Vector2(0, 1);
        }
        else if (left is not null)
        {
            current = left;
            dir = new Vector2(-1, 0);
        }
        else if (right is not null)
        {
            current = right;
            dir = new Vector2(1, 0);
        }
        else
        {
            throw new Exception();
        }

        return (current, dir);
    }

    private static int? NavigateThrough(string[] lines, Vector2 current, Vector2 dir, Vector2? extraWall = null)
    {
        var seen = new Dictionary<Vector2, HashSet<Vector2>>();
        while (lines.IsInRange(current))
        {
            var next = current + dir;
            if (lines.HasElementAt('#', next) || next == extraWall)
            {
                dir = dir.RotateClockwise();
            }
            else
            {
                if (!seen.ContainsKey(current)) seen[current] = new HashSet<Vector2>();
                if (seen[current].Contains(dir)) return null;
                seen[current].Add(dir);
                current = next;
            }
        }

        return seen.Keys.Count;
    }
}