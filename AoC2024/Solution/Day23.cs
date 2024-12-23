namespace AoC2024.Solution;

public static class Day23
{
    public static string Part2_2()
    {
        var conMap = GetInput();
        var keys = conMap.Keys.ToList();
        var subnets = keys.Select(k => new HashSet<string> { k }).ToList();
        foreach (var key in keys)
        {
            foreach (var subnet in subnets)
            {
                if (subnet.All(k => conMap[k].Contains(key)))
                    subnet.Add(key);
            }
        }

        return string.Join(",", subnets.MaxBy(s => s.Count)!.Order());
    }
    
    public static string Part2()
    {
        var conMap = GetInput();
        var allKeys = conMap.Keys.ToList();
        var subnets = GetThreeConnected(conMap)
            .Select(k => new HashSet<string> { k.Item1, k.Item2, k.Item3 })
            .ToDictionary(s => string.Join(",", s.Order().ToList()));

        var queue = new Queue<HashSet<string>>(subnets.Values);
        while (queue.Any())
        {
            Console.WriteLine(queue.Count);
            var current = queue.Dequeue();
            foreach (var el in allKeys.Where(k => !current.Contains(k)))
            {
                if (current.All(c => conMap[el].Contains(c)))
                {
                    var newSubnet = current.Append(el).ToHashSet();
                    var subnetKey = string.Join(",", newSubnet.Order().ToList());
                    if (!subnets.TryAdd(subnetKey, newSubnet)) continue;
                    queue.Enqueue(newSubnet);
                }
            }
        }


        var maxCount = subnets.Values.Max(s => s.Count);
        var ordered = subnets.Where(s => s.Value.Count == maxCount)
            .Select(s => s.Key)
            .Order()
            .ToList();
        return ordered
            .First();
    }

    private static Dictionary<string, HashSet<string>> GetInput()
    {
        var lines = File.ReadAllLines("Input/day23.txt");
        var connections = lines.Select(line => line.Split('-').ToPair()).ToList();
        var conMap = new Dictionary<string, HashSet<string>>();
        foreach (var connection in connections)
        {
            var (a, b) = connection;
            conMap[a] = conMap.TryGetValue(a, out var a0) ? a0.Append(b).ToHashSet() : [b];
            conMap[b] = conMap.TryGetValue(b, out var b0) ? b0.Append(a).ToHashSet() : [a];
        }

        return conMap;
    }

    public static int Part1()
    {
        var conMap = GetInput();
        var results = GetThreeConnected(conMap);

        return results
            .Count(x => x.Item1.StartsWith("t") || x.Item2.StartsWith("t") || x.Item3.StartsWith("t"));
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