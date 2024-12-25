using AoC2024.Utils;

namespace AoC2024.Solution;

public class Day02 : SolutionBase
{
    public override string Part1()
    {
        var reports = Lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        return reports.Count(IsSafe).ToString();
    }

    public override string Part2()
    {
        var reports = Lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        return reports.Count(IsSafe2).ToString();
    }

    private static bool IsSafe(List<int> report)
    {
        var dir = 0;
        for (int i = 1; i < report.Count; i++)
        {
            var curDif = report[i] - report[i - 1];
            if (Math.Abs(curDif) < 1 || Math.Abs(curDif) > 3) return false;
            if (dir == 0)
            {
                dir = curDif > 0 ? 1 : -1;
            }
            else if (curDif * dir < 0) return false;
        }

        return true;
    }

    private static bool IsSafe2(List<int> report) =>
        IsSafeForDirection(report, 1, true)
        || IsSafeForDirection(report, -1, true);

    private static bool IsSafeForDirection(List<int> report, int dir, bool noErrors)
    {
        for (int i = 1; i < report.Count; i++)
        {
            var curDif = report[i] - report[i - 1];
            if (Math.Abs(curDif) < 1 || Math.Abs(curDif) > 3)
            {
                return noErrors
                       && (IsSafeForDirection(report.RemoveAtIndex(i), dir, false)
                           || IsSafeForDirection(report.RemoveAtIndex(i - 1), dir, false));
            }

            if (curDif * dir < 0)
            {
                return noErrors
                       && (IsSafeForDirection(report.RemoveAtIndex(i), dir, false)
                           || IsSafeForDirection(report.RemoveAtIndex(i - 1), dir, false));
            }
        }

        return true;
    }
}