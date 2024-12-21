namespace AoC2024.Solution;

public static class Day16
{
    public static int Part1()
    {
        var map = File.ReadAllLines("Input/day16.txt");
        var start = map.FindPosition('S')!.Value;
        var startDir = Directions.Right;
        var end = map.FindPosition('E')!.Value;
        var seen = new Dictionary<(Vector2, Vector2), int> { { (start, startDir), 0 } };
        var q = new Queue<(Vector2, Vector2, int)>();
        q.Enqueue((start, startDir, 0));
        while (q.Count > 0)
        {
            var (current, dir, score) = q.Dequeue();
            if (current == end) continue;
            var nextList = new List<(Vector2, Vector2, int)>();
            if (!map.HasElementAt('#', current + dir)) nextList.Add((current + dir, dir, score + 1));
            nextList.Add((current, dir.RotateClockwise(), score + 1000));
            nextList.Add((current, dir.RotateCounterClockwise(), score + 1000));
            foreach (var next in nextList.Where(n =>
                     {
                         var key = (n.Item1, n.Item2);
                         return !seen.ContainsKey(key) || n.Item3 < seen[key];
                     }))
            {
                var key = (next.Item1, next.Item2);
                seen[key] = next.Item3;
                q.Enqueue(next);
            }
        }

        List<int> results = [];
        if (seen.TryGetValue((end, Directions.Right), out var right)) results.Add(right);
        if (seen.TryGetValue((end, Directions.Left), out var left)) results.Add(left);
        if (seen.TryGetValue((end, Directions.Up), out var up)) results.Add(up);
        if (seen.TryGetValue((end, Directions.Down), out var down)) results.Add(down);
        return results.Min();
    }

    public static int Part2()
    {
        var map = File.ReadAllLines("Input/day16.txt");
        var start = map.FindPosition('S')!.Value;
        var startDir = Directions.Right;
        var end = map.FindPosition('E')!.Value;
        var seen = new Dictionary<(Vector2, Vector2), (int, HashSet<Vector2>)> { { (start, startDir), (0, [start]) } };
        var q = new Queue<(Vector2, Vector2, int, HashSet<Vector2>)>();
        q.Enqueue((start, startDir, 0, [start]));
        while (q.Count > 0)
        {
            Console.WriteLine(q.Count());
            var (current, dir, score, path) = q.Dequeue();
            if (current == end) continue;
            
            var nextList = new List<(Vector2, Vector2, int, HashSet<Vector2>)>();
            if (!map.HasElementAt('#', current + dir))
                nextList.Add((current + dir, dir, score + 1, path.Append(current + dir).ToHashSet()));
            nextList.Add((current, dir.RotateClockwise(), score + 1000, path));
            nextList.Add((current, dir.RotateCounterClockwise(), score + 1000, path));
            foreach (var next in nextList.Where(n =>
                     {
                         var key = (n.Item1, n.Item2);
                         return !seen.ContainsKey(key) || n.Item3 <= seen[key].Item1;
                     }))
            {
                var key = (next.Item1, next.Item2);
                if (!seen.ContainsKey(key) || next.Item3 < seen[key].Item1)
                {
                    seen[key] = (next.Item3, next.Item4);
                }
                else
                {
                    seen[key] = (next.Item3, seen[key].Item2.Concat(next.Item4).ToHashSet());
                }

                q.Enqueue(next);
            }
        }
        
        List<(int, HashSet<Vector2>)> results = [];
        if (seen.TryGetValue((end, Directions.Right), out var right)) results.Add(right);
        if (seen.TryGetValue((end, Directions.Left), out var left)) results.Add(left);
        if (seen.TryGetValue((end, Directions.Up), out var up)) results.Add(up);
        if (seen.TryGetValue((end, Directions.Down), out var down)) results.Add(down);
        var min = results.Select(e => e.Item1).Min();
        var visited = results
            .Where(e => e.Item1 == min)
            .SelectMany(e => e.Item2)
            .ToHashSet();

        // for (var y = 0; y < map.Length; y++)
        // {
        //     var line = map[y];
        //     for (var x = 0; x < line.Length; x++)
        //     {
        //         var ch = line[x];
        //         if (visited.Contains(new Vector2(x, y))) Console.Write('O');
        //         else Console.Write(ch);
        //     }
        //     
        //     Console.WriteLine();
        // }
        
        return visited.Count;
    }
}