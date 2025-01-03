using AoC2024.Utils;

namespace AoC2024.Solution;

public  class Day04 : SolutionBase
{
    private readonly List<Vector2> _directions =
    [
        new(-1, 0), new(-1, -1), new(-1, 1),
        new(0, -1), new(0, 1),
        new(1, -1), new(1, 0), new(1, 1)
    ];
    
    public override string Part1()
    {
        
        var res = 0;
        var word = "XMAS";
        Lines.ForEachWithPos((el, current) =>
        {
            if (el == 'X')
            {
                foreach (var direction in _directions)
                {
                    var matches = true;
                    for (int i = 0; i < word.Length; i++)
                    {
                        var pos = current + direction * i;
                        if (!Lines.HasElementAt(word[i], pos))
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (matches) res++;
                }
            }

        });

        return res.ToString();
    }
    
    public override string Part2()
    {
        var res = 0;
        Lines.ForEachWithPos((el, current) =>
        {
            if (el != 'A') return;
            var one =
                Lines.HasElementAt('M', current + new Vector2(-1, -1)) &&
                Lines.HasElementAt('S', current + new Vector2(1, 1)) ||
                Lines.HasElementAt('S', current + new Vector2(-1, -1)) &&
                Lines.HasElementAt('M', current + new Vector2(1, 1));
            var two =
                Lines.HasElementAt('M', current + new Vector2(-1, 1)) &&
                Lines.HasElementAt('S', current + new Vector2(1, -1)) ||
                Lines.HasElementAt('S', current + new Vector2(-1, 1)) &&
                Lines.HasElementAt('M', current + new Vector2(1, -1));
            if (one && two) res++;

        });

        return res.ToString();
    }
}