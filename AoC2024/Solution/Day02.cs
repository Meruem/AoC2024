namespace AoC2024.Solution;

public static class Day02
{
    public static int Part1()
    {
        var lines = File.ReadAllLines("Input/day02.txt");
        var reports = lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        return reports.Count(IsSafe);
    }

    public static int Part2()
    {
        var lines = File.ReadAllLines("Input/day02.txt");
        var reports = lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
        return reports.Count(IsSafe2);
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