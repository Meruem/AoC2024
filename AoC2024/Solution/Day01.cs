namespace AoC2024.Solution;

public static class Day01
{
    public static int Part1()
    {
        var (left, right) = GetInputs();
        left.Sort();
        right.Sort();

        var result = 0;
        for (int i = 0; i < left.Count; i++)
        {
            result += Math.Abs(left[i] - right[i]);
        }

        return result;
    }
    
    public static int Part2()
    {
        var (left, right) = GetInputs();
        var rightCounts = right.Aggregate(new Dictionary<int, int>(),
            (acc, i) =>
            {
                acc[i] = acc.TryGetValue(i, out var value) ? value + 1 : 1;
                return acc;
            });

        var result = left.Sum(t => t * rightCounts.GetValueOrDefault(t));
        return result;
    }

    private static (List<int> left, List<int> right) GetInputs()
    {
        var lines = File.ReadLines("Input/day01.txt");
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in lines)
        {
            var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(splits[0]));
            right.Add(int.Parse(splits[1]));
        }

        return (left, right);
    }
}