namespace AoC2024.Solution;

public static class Day20
{
    public static int Part1() => Solve(2);
    public static int Part2() => Solve(20);

    private static int Solve(int radius)
    {
        var map = File.ReadAllLines("Input/day20.txt");
        var start = map.FindPosition('S')!.Value;
        var end = map.FindPosition('E')!.Value;

        var current = start;
        var distance = 0;
        var seen = new Dictionary<Vector2, int> { { start, distance } };
        while (current != end)
        {
            distance++;
            var next = Directions.AllDirections
                .Select(dir => dir + current)
                .Where(dir => !map.HasElementAt('#', dir) && !seen.ContainsKey(dir)).ToList();
            current = next.First();
            seen.Add(current, distance);
        }

        var result = seen.SelectMany(kvp =>
        {
            var pos = kvp.Key;
            var dist = kvp.Value;

            var cheats = new List<Vector2>();
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (Math.Abs(x) + Math.Abs(y) > radius) continue;
                var p = pos + new Vector2(x, y);
                if (seen.ContainsKey(p) && p != pos)
                    cheats.Add(p);
            }

            return cheats
                .Select(ch => seen[ch] - dist - (ch - pos).Distance)
                .Where(d => d > 0)
                .ToList();
        });
        
        return result.Count(cd => cd >= 100);
    }
}