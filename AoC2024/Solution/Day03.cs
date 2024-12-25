using System.Text.RegularExpressions;
using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day03 : SolutionBase
{
    public override string Part1()
    {
        var matches = Regex.Matches(InputText, @"(mul\((\d*),(\d*)\))");
        return matches.Sum(match =>
            int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value)
        ).ToString();
    }

    private static int CountMuls(string input)
    {
        var matches = Regex.Matches(input, @"(mul\((\d*),(\d*)\))");
        return matches.Sum(match =>
            int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value)
        );
    }
    
    public override string Part2()
    {
        var lines = InputText;
        var res = 0;

        while (lines.Length > 0)
        {
            var dontIndex = lines.IndexOf("don't()", StringComparison.Ordinal);
            if (dontIndex == -1)
            {
                res += CountMuls(lines);
                return res.ToString();
            }
            var enabled = lines.Substring(0, dontIndex);
            res += CountMuls(enabled);
            
            lines = lines.Substring(dontIndex + 1);
            var doIndex = lines.IndexOf("do()", StringComparison.Ordinal);
            if (doIndex == -1) return res.ToString();
            lines = lines.Substring(doIndex + 1);
        }

        return res.ToString();
    }
}