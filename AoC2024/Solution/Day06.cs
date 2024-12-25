using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day06: SolutionBase
{
    public override string Part1()
    {
        var grid = Grid.FromLines(Lines);
        var (current, dir) = GetStartPositionAndDir(grid);
        return NavigateThrough(grid, current, dir).Count.ToString();
    }

    public override string Part2()
    {
        var grid = Grid.FromLines(Lines);
        var (start, dir) = GetStartPositionAndDir(grid);
        var positions = NavigateThrough(grid, start, dir);
        var sum = 0;
        foreach (var pos in positions)
        {
            if (pos == start) continue;
            if (NavigateThrough(grid, start, dir, pos).Count == 0) sum++;
        }
        return sum.ToString();
    }

    private static (Vector2 current, Vector2 dir) GetStartPositionAndDir(Grid grid)
    {
        Vector2 current;
        Vector2 dir;
        var up = grid.FindPosition('^');
        var down = grid.FindPosition('v');
        var left = grid.FindPosition('<');
        var right = grid.FindPosition('>');
        if (up is not null)
        {
            current = up.Value;
            dir = new Vector2(0, -1);
        }
        else if (down is not null)
        {
            current = down.Value;
            dir = new Vector2(0, 1);
        }
        else if (left is not null)
        {
            current = left.Value;
            dir = new Vector2(-1, 0);
        }
        else if (right is not null)
        {
            current = right.Value;
            dir = new Vector2(1, 0);
        }
        else
        {
            throw new Exception();
        }

        return (current, dir);
    }

    private static List<Vector2> NavigateThrough(Grid grid, Vector2 current, Vector2 dir, Vector2? extraWall = null)
    {
        var seen = new Dictionary<Vector2, HashSet<Vector2>>();
        while (grid.IsInRange(current))
        {
            var next = current + dir;
            if (grid.HasElementAt('#', next) || next == extraWall)
            {
                dir = dir.RotateClockwise();
            }
            else
            {
                if (!seen.ContainsKey(current)) seen[current] = new HashSet<Vector2>();
                if (seen[current].Contains(dir)) return [];
                seen[current].Add(dir);
                current = next;
            }
        }

        return seen.Keys.ToList();
    }
}