using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day07 : SolutionBase
{
    public override string Part1()
    {
        return Lines.Sum(line =>
        {
            var parts = line.Split(": ");
            var target = long.Parse(parts[0]);
            var elements = parts[1].Split(" ").Select(long.Parse).ToList();
            return IsValid(target, elements) ? target : 0;
        })
        .ToString();
    }
    
    public override string Part2()
    {
        return Lines.Sum(line =>
        {
            var parts = line.Split(": ");
            var target = long.Parse(parts[0]);
            var elements = parts[1].Split(" ").Select(long.Parse).ToList();
            return IsValid2(target, elements) ? target : 0;
        })
        .ToString();
    }

    private static bool IsValid(long target, List<long> elements)
    {
        if (elements.Count == 1) return target == elements[0];
        var last = elements.Last();
        var sumTarget = target - last;
        var mulPossible = target % last == 0;
        var newElements = elements.Take(elements.Count - 1).ToList();
        return (sumTarget > 0 && IsValid(sumTarget, newElements)) ||
               (mulPossible && IsValid(target / last, newElements));
    }

    private static bool IsValid2(long target, List<long> elements)
    {
        if (elements.Count == 1) return target == elements[0];
        var last = elements.Last();
        var newElements = elements.Take(elements.Count - 1).ToList();

        var sumTarget = target - last;
        var sumPossible = sumTarget > 0;
        
        var mulPossible = target % last == 0;

        var targetStr = target.ToString();
        var appPossible = targetStr.EndsWith(last.ToString());
        var appTarget = appPossible 
            ? targetStr.Substring(0, targetStr.Length - last.ToString().Length) 
            : "";

        return (sumPossible && IsValid2(sumTarget, newElements)) ||
               (mulPossible && IsValid2(target / last, newElements)) ||
               (appPossible && IsValid2(appTarget == "" ? 0  : long.Parse(appTarget), newElements));
    }
}