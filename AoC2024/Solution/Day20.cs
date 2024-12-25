using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day20 : SolutionBase
{
    public override string Part1() => Solve(2).ToString();
    public override string Part2() => Solve(20).ToString();

    private int Solve(int radius)
    {
        var map = Grid.FromLines(Lines);
        var start = map.FindPosition('S')!.Value;
        var end = map.FindPosition('E')!.Value;

        var current = start;
        var distance = 0;
        var path = new Dictionary<Vector2, int> { { start, distance } };
        var direction = Directions.AllDirections.First(dir => !map.HasElementAt('#', dir));
        while (current != end)
        {
            distance++;
            var next = new[] { direction, direction.RotateClockwise(), direction.RotateCounterClockwise() }
                .Select(dir => dir + current)
                .First(dir => !map.HasElementAt('#', dir));
            direction = next - current;
            current = next;
            path[current] = distance;
        }

        int result = 0;
        Parallel.ForEach(path, kvp =>
        {
            var pos = kvp.Key;
            var dist = kvp.Value;
            var c = 0;
            
            for (int x = 2; x <= radius; x++)
            {
                var p = pos + new Vector2(x, 0);
                if (path.ContainsKey(p) &&
                    Math.Abs(path[p] - dist) - (p - pos).Distance >= 100)
                    c++;
       
            }

            for (int y = 1; y <= radius; y++)
            for (int x = y-radius; x <= radius - y; x++)
            {
                var p = pos + new Vector2(x, y);
                if (path.ContainsKey(p) &&
                    Math.Abs(path[p] - dist) - (p - pos).Distance >= 100)
                    c++;
            }
            
            Interlocked.Add(ref result, c);
        });

        return result;
    }
}