using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day05 : SolutionBase
{
    public override string Part1()
    {
        var (rules, updates) = GetInputs();
        return updates.Sum(update => IsValid(update, rules) ? MidElement(update) : 0).ToString();
    }

    public override string Part2()
    {
        var (rules, updates) = GetInputs();
        return updates.Sum(update => IsValid(update, rules) ? 0 : MidElement(Sort(update, rules))).ToString();
    }

    private List<int> Sort(List<int> update, List<Tuple<int, int>> rules) =>
        update.Order(Comparer<int>.Create((a, b) =>
        {
            foreach (var rule in rules)
            {
                if (rule.Item1 == a && rule.Item2 == b) return -1;
                if (rule.Item2 == a && rule.Item1 == b) return 1;
            }

            return 0;
        })).ToList();

    private (List<Tuple<int, int>> rules, List<List<int>> updates) GetInputs()
    {
        var parts = Lines.SplitBy("").ToList();
        var rules = parts[0].Select(line => line.Split("|").Select(int.Parse).ToPair()).ToList();
        var updates = parts[1].Select(line => line.Split(",").Select(int.Parse).ToList()).ToList();
        return (rules, updates);
    }

    private bool IsValid(List<int> update, List<Tuple<int, int>> rules) =>
        rules.All(rule =>
        {
            var aIndex = update.IndexOf(rule.Item1);
            var bIndex = update.IndexOf(rule.Item2);
            return aIndex == -1 || bIndex == -1 || aIndex <= bIndex;
        });

    private int MidElement(List<int> update) => update[update.Count / 2];
}