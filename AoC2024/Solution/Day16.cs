using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day16: SolutionBase
{
    public override string Part1() => Solve().Item1.ToString();
    public override string Part2() => Solve().Item2.ToString();
    
    private (int, int) Solve()
    {
        var map = Lines;
        var start = map.FindPosition('S')!.Value;
        var startDir = Directions.Right;
        var end = map.FindPosition('E')!.Value;

        var seen = new Dictionary<Vector2, Dictionary<Vector2, int>>
        {
            [start] = new() { {startDir, 0 } }
        };
        
        var q = new Queue<(Vector2, Vector2, int)>();
        q.Enqueue((start, startDir, 0));
        var lowest = int.MaxValue;
        while (q.Count > 0)
        {
            var (current, dir, score) = q.Dequeue();
            
            if (score >= lowest) continue;
            
            if (current == end)
            {
                lowest = score;
                continue;
            }
            
            var nextList = new List<(Vector2, Vector2, int)>();
            if (!map.HasElementAt('#', current + dir))
                nextList.Add((current + dir, dir, score + 1));
            nextList.Add((current, dir.RotateClockwise(), score + 1000));
            nextList.Add((current, dir.RotateCounterClockwise(), score + 1000));
            
            foreach (var next in nextList)
            {
                var (nextPos, nextDir, nextScore) = next;
                if (seen.TryGetValue(nextPos, out var dirDict))
                {
                    if (!dirDict.TryGetValue(nextDir, out var currentScore))
                    {
                        seen[nextPos][nextDir] = nextScore;
                        q.Enqueue(next);
                    }
                    else if (currentScore > nextScore)
                    {
                        dirDict[nextDir] = nextScore;
                        q.Enqueue(next);
                    }
                }
                else
                {
                    seen[nextPos] = new() { { nextDir, nextScore } };
                    q.Enqueue(next);
                }
            }
        }

        var results = seen[end];
        var minNode = results.MinBy(kvp => kvp.Value);
        var minValue = minNode.Value;
        var minNodes = results.Where(kvp => kvp.Value == minValue).ToList();
        
        var qBack = new Queue<(Vector2, Vector2, int)>(minNodes.Select(kvp => (end, kvp.Key, kvp.Value)));
        var path = new HashSet<Vector2>();

        while (qBack.Count > 0)
        {
            var (pos, dir, score) = qBack.Dequeue();
            path.Add(pos);
            if (pos == start) continue;

            var next = new[]
            {
                (pos - dir, dir, score - 1),
                (pos, dir.RotateClockwise(), score - 1000),
                (pos, dir.RotateCounterClockwise(), score - 1000),
            };

            foreach (var n in next)
            {
                var (nPos, nDir, nScore) = n;
                if (seen.TryGetValue(nPos, out var dirDict) &&
                    dirDict.TryGetValue(nDir, out var nextScore) && nextScore == nScore)
                {
                    dirDict.Remove(nDir);
                    qBack.Enqueue(n);
                }
            }
        }
        
        return (lowest, path.Count);
    }
}