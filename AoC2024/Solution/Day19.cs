namespace AoC2024.Solution;

public class Node
{
    public Dictionary<char, Node> Next { get; } = new();
    public bool Accepts { get; set; } = false;
    public string Path { get; set; } = "";
}

public static class Day19
{
    public static long Part1()
    {
        var lines = File.ReadLines("Input/day19.txt").ToList();
        var parts = lines.SplitBy("").ToList();
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
            Console.WriteLine($"{count} / {parts[1].Count}");
            count++;
            var currentStates = new Dictionary<Node, long> { {startNode ,1} };
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

                //Console.WriteLine(currentStates.Count);
                if (currentStates.Count == 0)
                {
                    break;
                }
            }

            // if (currentStates.ContainsKey(startNode)) result++; // part1
            result += currentStates.GetValueOrDefault(startNode);
        }

        return result;
    }
}