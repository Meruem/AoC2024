namespace AoC2024.Solution;

public static class Day21
{
    static List<char> keypadChars = ['<', '>', '^', 'v', 'A'];
    static List<char> directions = ['<', '>', '^', 'v'];
    static List<char> numericChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A'];
    
    static Dictionary<(char, char), (long, List<char>)> keyPrice0 = [];
    static Dictionary<(char, char), (long, List<char>)> numericPrice = [];

    private static Dictionary<(char, char), (long, List<char>)> Iterate(
        Dictionary<(char, char), (long, List<char>)> prices)
    {
        var result = new Dictionary<(char, char), (long, List<char>)>();
        foreach (var ch1 in keypadChars)
        foreach (var ch2 in keypadChars)
        {
            long GetPrice(char prev, char current) => prices[(prev, current)].Item1;
            var there = ShortestPath(ch1, ch2, GetPrice, MoveDirection);
            result[(ch1, ch2)] = there;
        }

        return result;
    }

    public static long Part1() => Solve(2);
    public static long Part2() => Solve(25);

    public static long Solve(int iterations)
    {
        foreach (var ch1 in keypadChars)
        foreach (var ch2 in keypadChars)
        {
            keyPrice0[(ch1, ch2)] = (1, [ch2]);
        }
        
        var prices = Enumerable.Range(1, iterations)
            .Aggregate(keyPrice0, (acc, _) => Iterate(acc));

        foreach (var ch1 in numericChars)
        foreach (var ch2 in numericChars)
        {
            long GetPrice(char prev, char current) => prices[(prev, current)].Item1;
            var there = ShortestPath(ch1, ch2, GetPrice, MoveNumeric);
            numericPrice[(ch1, ch2)] = there;
        }

        var input = File.ReadAllLines("Input/day21.txt");
        long res = 0;
        foreach (var line in input)
        {
            long sum = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (i == 0) sum += numericPrice[('A', line[i])].Item1;
                else sum += numericPrice[(line[i - 1], line[i])].Item1;
            }

            var nr = line.Remove(3).ToInt();

            Console.WriteLine($"{nr}: {sum} ");
            res += nr * sum;
        }

        return res;
    }

    private record Node(char Ch, long Len, char Dir);

    private static (long, List<char>) ShortestPath(
        char start,
        char end,
        Func<char, char, long> getPrice,
        Func<char, char, char?> move)
    {
        var seen = new Dictionary<(char, char), (long, List<char>)>();
        var queue = new Queue<(char, long, List<char>)>();
        queue.Enqueue((start, 0, []));
        while (queue.Count > 0)
        {
            var (current, len, path) = queue.Dequeue();
            if (current == end) continue;

            var previous = path.Count > 0 ? path[^1] : 'A';
            var nextItems = directions
                .Select(dir => (dir, move(dir, current)))
                .Where(n => n.Item2 is not null)
                .Select(n =>
                {
                    var dir = n.Item1;
                    var nextCh = n.Item2!.Value;
                    var price = getPrice(previous, dir);
                    return new Node(nextCh, price, dir);
                })
                .Where(el => !seen.TryGetValue((el.Dir, el.Ch), out var n) || n.Item1 > len + el.Len)
                .ToList();

            foreach (var n in nextItems)
            {
                seen[(n.Dir, n.Ch)] = (n.Len + len, path.Append(n.Dir).ToList());
                queue.Enqueue((n.Ch, n.Len + len, path.Append(n.Dir).ToList()));
            }
        }

        var c = seen.Where(kvp => kvp.Key.Item2 == end)
            .Select(kvp => (kvp.Value.Item1 + getPrice(kvp.Key.Item1, 'A'), kvp.Value.Item2.Append('A').ToList()))
            .ToList();

        return c.Count > 0 ? c.MinBy(k => k.Item1) : (1, ['A']);
    }

    private static char? MoveNumeric(char direction, char actual) => (direction, actual) switch
    {
        ('<', '2') => '1',
        ('<', '3') => '2',
        ('<', '5') => '4',
        ('<', '6') => '5',
        ('<', '8') => '7',
        ('<', '9') => '8',
        ('<', 'A') => '0',
        ('>', '0') => 'A',
        ('>', '1') => '2',
        ('>', '2') => '3',
        ('>', '4') => '5',
        ('>', '5') => '6',
        ('>', '7') => '8',
        ('>', '8') => '9',
        ('^', '0') => '2',
        ('^', '1') => '4',
        ('^', '2') => '5',
        ('^', '3') => '6',
        ('^', '4') => '7',
        ('^', '5') => '8',
        ('^', '6') => '9',
        ('^', 'A') => '3',
        ('v', '2') => '0',
        ('v', '3') => 'A',
        ('v', '4') => '1',
        ('v', '5') => '2',
        ('v', '6') => '3',
        ('v', '7') => '4',
        ('v', '8') => '5',
        ('v', '9') => '6',
        _ => null
    };

    private static char? MoveDirection(char direction, char actual) => (direction, actual) switch
    {
        ('<', '>') => 'v',
        ('<', 'v') => '<',
        ('<', 'A') => '^',
        ('>', '<') => 'v',
        ('>', '^') => 'A',
        ('>', 'v') => '>',
        ('^', '>') => 'A',
        ('^', 'v') => '^',
        ('v', '^') => 'v',
        ('v', 'A') => '>',
        _ => null
    };
}