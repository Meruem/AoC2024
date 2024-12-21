namespace AoC2024.Solution;

public static class Day18
{
    private static bool IsInside(Vector2 pos, int maxX, int maxY) =>
        pos.X <= maxX && pos.X >= 0 && pos.Y <= maxY && pos.Y >= 0;


    public static int Part1()
    {
        var lines = File.ReadLines(@"Input/day18.txt");
        var positions = lines.Select(line =>
        {
            var splits = line.Split(",");
            return new Vector2(int.Parse(splits[0]), int.Parse(splits[1]));
        }).ToList();

        var walls = positions.Take(1024).ToList();
        return FindPath(walls)!.Count - 1;
    }
    
    public static string Part2()
    {
        //2994
        var lines = File.ReadLines(@"Input/day18.txt");
        var positions = lines.Select(line =>
        {
            var splits = line.Split(",");
            return new Vector2(int.Parse(splits[0]), int.Parse(splits[1]));
        }).ToList();

        var previous = new List<Vector2>();
        for (int i = 1024; i < positions.Count; i++)
        {
            Console.WriteLine(i);
            if (previous.Count > 0 && !previous.Contains(positions[i-1])) continue;
            var walls = positions.Take(i).ToList();
            var res = FindPath(walls);
            if (res == null) return positions[i].ToString();
            previous = res;
        }
        return "na";
    }

    private static List<Vector2>? FindPath(List<Vector2> walls)
    {
        var maxX = 70;
        var maxY = 70;
        var end = new Vector2(maxX, maxY);
        var start = new Vector2(0, 0);
        Dictionary<Vector2, List<Vector2>> seen = new();
        var q = new Queue<(Vector2, List<Vector2>)>();
        q.Enqueue((start, [start]));
        while (q.Count > 0)
        {
            var current = q.Dequeue();
            if (current.Item1 == end) continue;
            var neighbours = Directions.AllDirections
                .Select(dir => current.Item1 + dir)
                .Where(next => IsInside(next, maxX, maxY) 
                               && !walls.Contains(next)
                               && (!seen.ContainsKey(next) || seen[next].Count > (current.Item2.Count + 1)))
                .ToList();
            foreach (var neighbour in neighbours)
            {
                var newPath = current.Item2.Append(neighbour).ToList();
                seen[neighbour] = newPath;
                q.Enqueue((neighbour, newPath));
            }
        }

        return seen.GetValueOrDefault(end);
    }
}