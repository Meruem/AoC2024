using System.Text;
using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day15 : SolutionBase
{
    public override string Part1()
    {
        var (map, commands) = GetInputs();

        var pos = map.FindPosition('@')!.Value;
        foreach (var command in commands)
        {
            var dir = Directions.DirectionMap[command];
            var next = dir + pos;
            if (map.HasElementAt('.', next))
            {
                map.SetElementAt('.', pos);
                map.SetElementAt('@', next);
                pos = next;
            }
            else if (map.HasElementAt('O', next))
            {
                var nextFree = next + dir;
                while (map.HasElementAt('O', nextFree)) nextFree += dir;
                if (map.HasElementAt('.', nextFree))
                {
                    map.SetElementAt('O', nextFree);
                    map.SetElementAt('@', next);
                    map.SetElementAt('.', pos);
                    pos = next;
                }
            }
        }

        return GetResult(map).ToString();
    }

    public override string Part2()
    {
        var (map, commands) = GetInputs();
        map = map.Select(line =>
        {
            var newLine = new StringBuilder();
            foreach (var ch in line)
            {
                switch (ch)
                {
                    case '#':
                        newLine.Append("##");
                        break;
                    case 'O':
                        newLine.Append("[]");
                        break;
                    case '.':
                        newLine.Append("..");
                        break;
                    case '@':
                        newLine.Append("@.");
                        break;
                }
            }

            return newLine.ToString();
        }).ToList();

        var pos = map.FindPosition('@')!.Value;
        foreach (var command in commands)
        {
            var dir = Directions.DirectionMap[command];
            var next = dir + pos;
            var charAtNext = map.GetCharAt(next);
            switch (charAtNext)
            {
                case '.':
                    map.SetElementAt('.', pos);
                    map.SetElementAt('@', next);
                    pos = next;
                    break;
                case '[' or ']':
                {
                    var shouldMove = new HashSet<Tuple<Vector2, char>> { new(pos, '@') };
                    bool canMove = true;
                    var q = new Queue<Tuple<Vector2, char>>();
                    q.Enqueue(new Tuple<Vector2, char>(pos, '@'));
                    while (canMove && q.Count > 0)
                    {
                        var cur = q.Dequeue();
                        var n = cur.Item1 + dir;
                        var charAtN = map.GetCharAt(n);
                        if (charAtN == '#')
                        {
                            canMove = false;
                            break;
                        }

                        if (charAtN is '[' or ']')
                        {
                            var toAdd = new List<Tuple<Vector2, char>>();
                            var direct = new Tuple<Vector2, char>(n, charAtN);
                            toAdd.Add(direct);
                            if ((dir == Directions.Up || dir == Directions.Down) && charAtN == '[')
                                toAdd.Add(new Tuple<Vector2, char>(n + Directions.Right, ']'));
                            if ((dir == Directions.Up || dir == Directions.Down) && charAtN == ']')
                                toAdd.Add(new Tuple<Vector2, char>(n + Directions.Left, '['));
                            foreach (var x in toAdd)
                            {
                                if (!shouldMove.Contains(x))
                                {
                                    q.Enqueue(x);
                                    shouldMove.Add(x);
                                }
                            }
                        }
                    }

                    if (canMove)
                    {
                        foreach (var toMove in shouldMove)
                        {
                            map.SetElementAt('.', toMove.Item1);
                        }

                        foreach (var toMove in shouldMove)
                        {
                            map.SetElementAt(toMove.Item2, toMove.Item1 + dir);
                        }

                        pos = next;
                    }

                    break;
                }
            }
        }

        return GetResult(map).ToString();
    }

    private int GetResult(List<string> map)
    {
        var sum = 0;
        map.ForEachWithPos((ch, pos) =>
        {
            if (ch is 'O' or '[') sum += 100 * pos.Y + pos.X;
        });

        return sum;
    }

    private (List<string> map, List<char> commands) GetInputs()
    {
        var parts = Lines.SplitBy("").ToList();
        var map = parts[0];
        var commands = parts[1].SelectMany(x => x).ToList();
        return (map, commands);
    }
}