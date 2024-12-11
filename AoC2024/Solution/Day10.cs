namespace AoC2024.Solution;

public static class Day10
{
    private static readonly List<Vector2> Directions = [new(-1, 0), new(1, 0), new(0, 1), new(0, -1)];

    public static int Part1()
    {
        var map = File.ReadAllLines("Input/day10.txt");
        var res = 0;
        map.ForEachWithPos((ch, pos) =>
        {
            var val = ch.ToString().ToInt();
            if (val != 0) return;
            HashSet<Vector2> positions = [pos];
            var expectedVal = 1;
            while (positions.Count > 0 && expectedVal <= 9)
            {
                positions = positions
                    .SelectMany(p =>
                        Directions
                            .Select(d => d + p)
                            .Where(newPos =>
                                map.HasElementAt(expectedVal.ToString()[0], newPos))
                            .ToList())
                    .ToHashSet();
                expectedVal++;
            }
            if (expectedVal == 10) res+= positions.Count;
        });
        return res;
    }
    
    public static int Part2()
    {
        var map = File.ReadAllLines("Input/day10.txt");
        var res = 0;
        map.ForEachWithPos((ch, pos) =>
        {
            var val = ch.ToString().ToInt();
            if (val != 0) return;
            List<Vector2> positions = [pos];
            var expectedVal = 1;
            while (positions.Count > 0 && expectedVal <= 9)
            {
                positions = positions
                    .SelectMany(p =>
                        Directions
                            .Select(d => d + p)
                            .Where(newPos =>
                                map.HasElementAt(expectedVal.ToString()[0], newPos))
                            .ToList())
                    .ToList();
                expectedVal++;
            }
            if (expectedVal == 10) res+= positions.Count;
        });
        return res;
    }
}