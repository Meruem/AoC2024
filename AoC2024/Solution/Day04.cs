namespace AoC2024.Solution;

public static class Day04
{
    private static readonly List<Vector2> _directions =
    [
        new(-1, 0), new(-1, -1), new(-1, 1),
        new(0, -1), new(0, 1),
        new(1, -1), new(1, 0), new(1, 1)
    ];
    
    public static int Part1()
    {
        var lines = File.ReadAllLines(@"Input/day04.txt");
        
        var res = 0;
        var word = "XMAS";
        lines.ForEachWithPos((el, current) =>
        {
            if (el == 'X')
            {
                foreach (var direction in _directions)
                {
                    var matches = true;
                    for (int i = 0; i < word.Length; i++)
                    {
                        var pos = current + direction * i;
                        if (!lines.HasElementAt(word[i], pos))
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (matches) res++;
                }
            }

        });

        return res;
    }
    
    public static int Part2()
    {
        var lines = File.ReadAllLines(@"Input/day04.txt");
        
        var res = 0;
        lines.ForEachWithPos((el, current) =>
        {
            if (el != 'A') return;
            var one =
                lines.HasElementAt('M', current + new Vector2(-1, -1)) &&
                lines.HasElementAt('S', current + new Vector2(1, 1)) ||
                lines.HasElementAt('S', current + new Vector2(-1, -1)) &&
                lines.HasElementAt('M', current + new Vector2(1, 1));
            var two =
                lines.HasElementAt('M', current + new Vector2(-1, 1)) &&
                lines.HasElementAt('S', current + new Vector2(1, -1)) ||
                lines.HasElementAt('S', current + new Vector2(-1, 1)) &&
                lines.HasElementAt('M', current + new Vector2(1, -1));
            if (one && two) res++;

        });

        return res;
    }
}