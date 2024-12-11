using System.Text.RegularExpressions;

namespace AoC2024.Solution;

public static class Day03
{
    public static int Part1()
    {
        var lines = File.ReadAllText("Input/Day03.txt");
        var matches = Regex.Matches(lines, @"(mul\((\d*),(\d*)\))");
        return matches.Sum(match =>
            int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value)
        );
    }

    private static int CountMuls(string input)
    {
        var matches = Regex.Matches(input, @"(mul\((\d*),(\d*)\))");
        return matches.Sum(match =>
            int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value)
        );
    }
    
    public static int Part2()
    {
        var lines = File.ReadAllText("Input/Day03.txt");
        var res = 0;

        while (lines.Length > 0)
        {
            var dontIndex = lines.IndexOf("don't()", StringComparison.Ordinal);
            if (dontIndex == -1)
            {
                res += CountMuls(lines);
                return res;
            }
            var enabled = lines.Substring(0, dontIndex);
            res += CountMuls(enabled);
            
            lines = lines.Substring(dontIndex + 1);
            var doIndex = lines.IndexOf("do()", StringComparison.Ordinal);
            if (doIndex == -1) return res;
            lines = lines.Substring(doIndex + 1);
        }

        return res;
    }
}