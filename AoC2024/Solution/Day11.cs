namespace AoC2024.Solution;

public static class Day11
{
    public static long Part1() => Solve(25);
    public static long Part2() => Solve(75);
    private static long Solve(int blinks)
    {
        var input 
            = "0 89741 316108 7641 756 9 7832357 91"
            .Split(" ")
            .Select(long.Parse)
            .ToDictionary(i => i, i => 1L);
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
                
                // Console.WriteLine($"{blink}: {string.Join(',', next)} ");
                Console.WriteLine(blink);
            });

        return input.Sum(kvp => kvp.Value);
    }
}