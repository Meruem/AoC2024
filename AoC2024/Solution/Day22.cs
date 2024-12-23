namespace AoC2024.Solution;

public static class Day22
{
    public static long Part1()
    {
        var lines = File.ReadAllLines(@"Input/day22.txt");
        return lines.Sum(line => CalculateNth(long.Parse(line), 2000)[^1]);
    }

    public static long Part2()
    {
        var lines = File.ReadAllLines(@"Input/day22.txt");
        var changes = new Dictionary<(int, int, int, int), long>();
        foreach (var line in lines)
        {
            var prices = LastDigits(long.Parse(line), 2000);
            var deltas = prices.Skip(1).Select((p, i) => p - prices[i]).ToList();
            var changePrices = new Dictionary<(int, int, int, int), long>();
            for (int i = 3; i < deltas.Count; i++)
            {
                var key = (deltas[i - 3], deltas[i - 2], deltas[i - 1], deltas[i]);
                if (changePrices.ContainsKey(key)) continue;
                changePrices[key] = prices[i + 1];
            }

            changes.MergeWith(changePrices);
        }

        return changes.Values.Max();
    }

    private static void MergeWith<TKey>(this Dictionary<TKey, long> first, Dictionary<TKey, long> second)
        where TKey : notnull
    {
        foreach (var kvp in second)
            first[kvp.Key] = first.GetValueOrDefault(kvp.Key) + kvp.Value;
    }

    private static List<byte> LastDigits(long s, int n) => CalculateNth(s, n).Select(LastDigit).ToList();
    private static byte LastDigit(long input) => (byte)(input % 10);

    private static List<long> CalculateNth(long s, int n) => Enumerable.Range(1, n)
        .Aggregate(new List<long> { s }, (acc, _) =>
        {
            acc.Add(Calculate(acc[^1]));
            return acc;
        });

    private static long Calculate(long s)
    {
        s = (s ^ (s << 6)) & 0xFFFFFF;
        s = s ^ (s >> 5);
        s = (s ^ (s << 11)) & 0xFFFFFF;
        return s;
    }
}