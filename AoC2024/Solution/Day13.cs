namespace AoC2024.Solution;

public static class Day13
{
        public static long Part2()
    {
        var lines = File.ReadAllLines("Input/day13.txt");
        var machines = lines.SplitBy("").ToList();
        long res = 0;
        foreach (var machine in machines)
        {
            var aStr = machine[0].Replace("Button A: ", "").Split(", ");
            var aX = long.Parse(aStr[0].Replace("X+", ""));
            var aY = long.Parse(aStr[1].Replace("Y+", ""));

            var bStr = machine[1].Replace("Button B: ", "").Split(", ");
            var bX = long.Parse(bStr[0].Replace("X+", ""));
            var bY = long.Parse(bStr[1].Replace("Y+", ""));

            var prize = machine[2].Replace("Prize: ", "").Split(", ");
            var prizeX = long.Parse(prize[0].Replace("X=", "")) + 10000000000000;
            var prizeY = long.Parse(prize[1].Replace("Y=", "")) + 10000000000000;

            decimal j = (prizeY - aY * (decimal)prizeX / aX) / (bY - (decimal)bX * aY / aX);
            long intJ = (long)Math.Round(j, 0);
            decimal i = (prizeX - j * bX) / aX;
            long intI = (long)Math.Round(i, 0);
            if (aX * intI + bX * intJ == prizeX && aY * intI + bY * intJ == prizeY)
            {
                long? min = 3 * intI + intJ;
                if (min != null) res += min.Value;
            }
        }

        return res;
    }
    
    public static int Part1()
    {
        var lines = File.ReadAllLines("Input/day13.txt");
        var machines = lines.SplitBy("").ToList();
        var res = 0;
        foreach (var machine in machines)
        {
            var aStr = machine[0].Replace("Button A: ", "").Split(", ");
            var aX = int.Parse(aStr[0].Replace("X+", ""));
            var aY = int.Parse(aStr[1].Replace("Y+", ""));

            var bStr = machine[1].Replace("Button B: ", "").Split(", ");
            var bX = int.Parse(bStr[0].Replace("X+", ""));
            var bY = int.Parse(bStr[1].Replace("Y+", ""));

            var prize = machine[2].Replace("Prize: ", "").Split(", ");
            var prizeX = int.Parse(prize[0].Replace("X=", ""));
            var prizeY = int.Parse(prize[1].Replace("Y=", ""));


            decimal j = (prizeY - aY * (decimal)prizeX / aX) / (bY - (decimal)bX * aY / aX);
            int intJ = (int)Math.Round(j, 0);
            decimal i = (prizeX - j * bX) / aX;
            int intI = (int)Math.Round(i, 0);
            if (aX * intI + bX * intJ == prizeX && aY * intI + bY * intJ == prizeY)
            {
                int? min = 3 * intI + intJ;
                if (min != null) res += min.Value;
            }
        }

        return res;
    }
}