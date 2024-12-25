using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day25 : SolutionBase
{
    private record Pin(int A, int B, int C, int D, int E)
    {
        public int Count => A + B + C + D + E;

        public bool FitPerfectly(Pin pin) =>
            A + pin.A == 5 && B + pin.B == 5 && C + pin.C == 5 && D + pin.D == 5 && E + pin.E == 5;
        public bool Fit(Pin pin) =>
            A + pin.A <= 5 && B + pin.B <= 5 && C + pin.C <= 5 && D + pin.D <= 5 && E + pin.E <= 5;
    }

    public override string Part2() => "Done!";
    public override string Part1()
    {
        var groups = Lines.SplitBy("").ToList();
        var allLocks = new List<Pin>();
        var allKeys = new List<Pin>();
        foreach (var g in groups)
        {
            var isKey = g[0].StartsWith(".");
            var counts = Enumerable.Range(0, g[0].Length).Select(_ => -1).ToList();
            foreach (var line in g)
            {
                for (int i = 0; i < line.Length; i++)
                    if (line[i] == '#')
                        counts[i]++;
            }

            var pin = new Pin(counts[0], counts[1], counts[2], counts[3], counts[4]);
            if (isKey) allKeys.Add(pin);
            else allLocks.Add(pin);
        }

        var keyDict = allKeys.GroupBy(k => k.Count)
            .ToDictionary(gr => gr.Key, gr => gr.ToList());
        var lockDict = allLocks.GroupBy(k => k.Count)
            .ToDictionary(gr => gr.Key, gr => gr.ToList());
        var result = 0L;
        foreach (var kvp in lockDict)
        {
            var locks = kvp.Value;
            var keys = keyDict
                .Where(pair => pair.Key <= 25 - kvp.Key)
                .SelectMany(e => e.Value).ToList();
            foreach (var lck in locks)
            {
                foreach (var key in keys)
                {
                    if (key.Fit(lck)) result++;
                }
            }
        }

        return result.ToString();
    }
}