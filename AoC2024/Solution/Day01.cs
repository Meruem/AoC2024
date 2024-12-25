using AoC2024.Utils;

namespace AoC2024.Solution;

public  class Day01 : SolutionBase
{
    public override string Part1()
    {
        var (left, right) = GetInputs();
        left.Sort();
        right.Sort();

        var result = 0;
        for (int i = 0; i < left.Count; i++)
        {
            result += Math.Abs(left[i] - right[i]);
        }

        return result.ToString();
    }
    
    public override string Part2()
    {
        var (left, right) = GetInputs();
        var rightCounts = right.Aggregate(new Dictionary<int, int>(),
            (acc, i) =>
            {
                acc[i] = acc.TryGetValue(i, out var value) ? value + 1 : 1;
                return acc;
            });

        var result = left.Sum(t => t * rightCounts.GetValueOrDefault(t));
        return result.ToString();
    }

    private (List<int> left, List<int> right) GetInputs()
    {
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in Lines)
        {
            var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(splits[0]));
            right.Add(int.Parse(splits[1]));
        }

        return (left, right);
    }
}