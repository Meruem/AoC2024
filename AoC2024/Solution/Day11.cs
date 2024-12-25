using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day11 : SolutionBase
{
    public override string Part1() => Solve(25).ToString();
    public override string Part2() => Solve(75).ToString();

    private long Solve(int blinks)
    {
        var input = InputText
            .Split(" ")
            .Select(long.Parse)
            .ToDictionary(i => i, _ => 1L);
        Enumerable.Range(1, blinks)
            .ForEach(blink =>
            {
                Dictionary<long, long> next = [];
                foreach (var kvp in input)
                {
                    if (kvp.Key == 0) next.AddOrSet(1, kvp.Value);
                    else if (kvp.Key.ToString().Length % 2 == 0)
                    {
                        var inputStr = kvp.Key.ToString();
                        var len = inputStr.Length / 2;
                        var left = inputStr.Substring(0, len);
                        var right = inputStr.Substring(len, len);
                        next.AddOrSet(long.Parse(left), kvp.Value);
                        next.AddOrSet(long.Parse(right), kvp.Value);
                    }
                    else
                    {
                        next.AddOrSet(kvp.Key * 2024, kvp.Value);
                    }
                }

                input = next;
            });

        return input.Sum(kvp => kvp.Value);
    }
}