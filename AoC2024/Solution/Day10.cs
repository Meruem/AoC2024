using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day10 : SolutionBase
{
    private static readonly List<Vector2> Directions = [new(-1, 0), new(1, 0), new(0, 1), new(0, -1)];

    public override string Part1()
    {
        var res = 0;
        Lines.ForEachWithPos((ch, pos) =>
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
                                Lines.HasElementAt(expectedVal.ToString()[0], newPos))
                            .ToList())
                    .ToHashSet();
                expectedVal++;
            }
            if (expectedVal == 10) res+= positions.Count;
        });
        return res.ToString();
    }
    
    public override string Part2()
    {
        var res = 0;
        Lines.ForEachWithPos((ch, pos) =>
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
                                Lines.HasElementAt(expectedVal.ToString()[0], newPos))
                            .ToList())
                    .ToList();
                expectedVal++;
            }
            if (expectedVal == 10) res+= positions.Count;
        });
        return res.ToString();
    }
}