using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day12 : SolutionBase
{
    private static readonly List<Vector2> Directions = [new(-1, 0), new(1, 0), new(0, 1), new(0, -1)];

    private int CountFences(HashSet<Vector2> fences, Vector2 direction)
    {
        var min = fences.Select(v => v.X).Concat(fences.Select(v2 => v2.Y)).Min() - 1;
        var max = fences.Select(v => v.X).Concat(fences.Select(v2 => v2.Y)).Max() + 1;

        var horizontal = direction.X == 0;

        var c = false;
        var fenceCount = 0;
        for (var x = min; x <= max; x++)
        for (var y = min; y <= max; y++)
        {
            if (fences.Contains(horizontal ? new Vector2(y, x) : new Vector2(x, y)))
            {
                if (!c) fenceCount++;
                c = true;
            }
            else
            {
                c = false;
            }
        }

        return fenceCount;
    }

    public override string Part1() => Solve(false).ToString();
    public override string Part2() => Solve(true).ToString();

    private int Solve(bool shrink)
    {
        var seen = new HashSet<Vector2>();
        var res = 0;
        Lines.ForEachWithPos((ch, pos) =>
        {
            if (seen.Contains(pos)) return;
            var todo = new Queue<Vector2>();
            todo.Enqueue(pos);
            var fences = new HashSet<Tuple<Vector2, Vector2>>();
            var visited = new HashSet<Vector2>();
            while (todo.Count > 0)
            {
                var current = todo.Dequeue();
                visited.Add(current);
                seen.Add(current);
                foreach (var dir in Directions)
                {
                    var next = dir + current;
                    if (Lines.HasElementAt(ch, next))
                    {
                        if (!visited.Contains(next))
                        {
                            todo.Enqueue(next);
                            visited.Add(next);
                        }
                    }
                    else
                    {
                        fences.Add(new(current, dir));
                    }
                }
            }

            int fenceCount;
            if (shrink)
            {
                var groups =
                    fences.GroupBy(v => v.Item2)
                        .ToList();

                fenceCount =
                    groups.Sum(grouping => CountFences(grouping.Select(g => g.Item1).ToHashSet(), grouping.Key));
            }
            else
            {
                fenceCount = fences.Count;
            }

            //Console.WriteLine($"visited: {visited.Count} fences: {fenceCount} ");
            res += fenceCount * visited.Count;
        });

        return res;
    }
}