using AoC2024.Utils;

namespace AoC2024.Solution;

public class Node
{
    public Dictionary<char, Node> Next { get; } = new();
    public bool Accepts { get; set; } = false;
    public string Path { get; set; } = "";
}

public class Day19 : SolutionBase
{
    public override string Part2() => Solve(false);
    public override string Part1() => Solve(true);
    
    private string Solve(bool isPart1)
    {
        var parts = Lines.SplitBy("").ToList();
        var designs = parts[0][0].Split(", ");
        var startNode = new Node();
        foreach (var design in designs)
        {
            var currNode = startNode;
            foreach (var ch in design)
            {
                if (!currNode.Next.ContainsKey(ch))
                {
                    currNode.Next[ch] = new Node { Path = currNode.Path + ch };
                }

                currNode = currNode.Next[ch];
            }

            currNode.Accepts = true;
        }

        long result = 0;
        var count = 1;
        foreach (var target in parts[1])
        {
            count++;
            var currentStates = new Dictionary<Node, long> { { startNode, 1 } };
            foreach (var ch in target)
            {
                var res = new Dictionary<Node, long>();
                foreach (var kvp in currentStates)
                {
                    var state = kvp.Key;
                    var c = kvp.Value;
                    if (state.Next.TryGetValue(ch, out var nextNode))
                    {
                        res.AddOrSet(nextNode, c);
                        if (nextNode.Accepts) res.AddOrSet(startNode, c);
                    }
                }

                currentStates = res;

                if (currentStates.Count == 0)
                {
                    break;
                }
            }

            if (isPart1)
            {
                if (currentStates.ContainsKey(startNode)) result++;
            }
            else
            {
                result += currentStates.GetValueOrDefault(startNode);
            }
        }

        return result.ToString();
    }
}