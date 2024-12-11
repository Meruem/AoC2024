namespace AoC2024.Solution;

public static class Day08
{
    public static int Part1()
    {
        var lines = File.ReadLines(@"Input/day08.txt").ToList();
        var antennas = GetAntennas(lines);

        var res = new HashSet<Vector2>();
        foreach (var group in antennas)
        {
            for (int i = 0; i < group.Value.Count - 1; i++)
            {
                var pos1 = group.Value[i].Item2;
                foreach (var second in group.Value.Skip(i + 1))
                {
                    var pos2 = second.Item2;
                    var diff = pos1 - pos2;
                    var a = pos2 - diff;
                    var b = pos1 + diff;
                    res.Add(a);
                    res.Add(b);
                }
            }
        }

        return res.Count(pos => lines.IsInRange(pos));
    }
    
    public static int Part2()
    {
        var lines = File.ReadLines(@"Input/day08.txt").ToList();
        var antennas = GetAntennas(lines);

        var res = new HashSet<Vector2>();
        foreach (var group in antennas)
        {
            for (int i = 0; i < group.Value.Count - 1; i++)
            {
                var pos1 = group.Value[i].Item2;
                foreach (var second in group.Value.Skip(i + 1))
                {
                    var pos2 = second.Item2;
                    var diff = pos1 - pos2;
                    var a = pos2 - diff;
                    res.Add(pos1);
                    res.Add(pos2);
                    while (lines.IsInRange(a))
                    {
                        res.Add(a);
                        a -= diff;
                    }
                    var b = pos1 + diff;
                    while (lines.IsInRange(b))
                    {
                        res.Add(b);
                        b += diff;
                    }
                }
            }
        }

        return res.Count;
    }

    private static Dictionary<char, List<Tuple<char, Vector2>>> GetAntennas(List<string> lines)
    {
        var antennas = 
            lines
                .ElementsWithPos()
                .Where(pair => pair.Item1 != '.')
                .GroupBy(t => t.Item1)
                .ToDictionary(g => g.Key, g => g.ToList());
        return antennas;
    }
}