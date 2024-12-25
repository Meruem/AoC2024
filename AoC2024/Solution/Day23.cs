using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day23 : SolutionBase
{
    public override string Part2()
    {
        var conMap = GetInput();
        var seen = new HashSet<string>();
        var largestClique = new List<string>();
        foreach (var kvp in conMap)
        {
            if (seen.Contains(kvp.Key)) continue;
            
            var clique = new List<string> { kvp.Key };
            foreach (var neighbour in kvp.Value)
            {
                var nn = conMap[neighbour];
                if (clique.All(c => nn.Contains(c)))
                {
                    clique.Add(neighbour);
                    seen.Add(neighbour);
                }
            }
            
            if (largestClique.Count < clique.Count) largestClique = clique;
        }

        return string.Join(",", largestClique.Order());
    }

    private Dictionary<string, HashSet<string>> GetInput()
    {
        var connections = Lines.Select(line => line.Split('-').ToPair()).ToList();
        var conMap = new Dictionary<string, HashSet<string>>();
        foreach (var connection in connections)
        {
            var (a, b) = connection;
            conMap[a] = conMap.TryGetValue(a, out var a0) ? a0.Append(b).ToHashSet() : [b];
            conMap[b] = conMap.TryGetValue(b, out var b0) ? b0.Append(a).ToHashSet() : [a];
        }

        return conMap;
    }

    public override string Part1()
    {
        var conMap = GetInput();
        var results = GetThreeConnected(conMap);

        return results
            .Count(x => x.Item1.StartsWith("t") || x.Item2.StartsWith("t") || x.Item3.StartsWith("t")).ToString();
    }

    private static HashSet<(string, string, string)> GetThreeConnected(Dictionary<string, HashSet<string>> conMap)
    {
        var results = new HashSet<(string, string, string)>();
        foreach (var pair in conMap)
        {
            var a = pair.Key;
            foreach (var b in pair.Value)
            foreach (var c in conMap[b])
                if (conMap[c].Contains(a))
                    results.Add(new List<string> { a, b, c }.Order().ToTriple());
        }

        return results;
    }
}