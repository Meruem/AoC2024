using System.Text.RegularExpressions;

namespace AoC2024.Solution;

public static class Day14
{
    public static int Part1()
    {
        var lines = File.ReadLines(@"Input/day14.txt");
        // var sizeX = 11;
        // var sizeY = 7;
        var sizeX = 101;
        var sizeY = 103;
        var iterations = 100;
        var positions = new List<Vector2>();
        foreach (var line in lines)
        {
            var regex = Regex.Matches(line, @"p=(\d*),(\d*) v=(-?\d*),(-?\d*)")[0].Groups;
            var p = new Vector2(regex[1].Value.ToInt(), regex[2].Value.ToInt());
            var vel = new Vector2(regex[3].Value.ToInt(), regex[4].Value.ToInt());
            p += vel * iterations;
            positions.Add(
                new Vector2(
                    p.X >= 0 ? p.X % sizeX : (sizeX - (-p.X % sizeX)) % sizeX,
                    p.Y >= 0 ? p.Y % sizeY : (sizeY - (-p.Y % sizeY)) % sizeY));
        }

        var g1 = positions.Where(p => p.X < sizeX / 2 && p.Y < sizeY / 2).ToList();
        var g2 = positions.Where(p => p.X < sizeX / 2 && p.Y > sizeY / 2).ToList();
        var g3 = positions.Where(p => p.X > sizeX / 2 && p.Y < sizeY / 2).ToList();
        var g4 = positions.Where(p => p.X > sizeX / 2 && p.Y > sizeY / 2).ToList();

        return g1.Count * g2.Count * g3.Count * g4.Count;
    }


    public static void Part2()
    {
        var lines = File.ReadLines(@"Input/day14.txt");
        var sizeX = 101;
        var sizeY = 103;
        var positions = new List<Tuple<Vector2, Vector2>>();
        foreach (var line in lines)
        {
            var regex = Regex.Matches(line, @"p=(\d*),(\d*) v=(-?\d*),(-?\d*)")[0].Groups;
            var p = new Vector2(regex[1].Value.ToInt(), regex[2].Value.ToInt());
            var vel = new Vector2(regex[3].Value.ToInt(), regex[4].Value.ToInt());
            positions.Add(new(p, vel));
            // p += vel * iterations;
            // positions.Add(
            //     new Vector2(
            //         p.X >= 0 ? p.X % sizeX : (sizeX - (-p.X % sizeX)) % sizeX,
            //         p.Y >= 0 ? p.Y % sizeY : (sizeY - (-p.Y % sizeY)) % sizeY));
        }

        int iterations=0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine(iterations);
            Console.WriteLine();

            var map = positions.Select(p => p.Item1).ToHashSet();
            if (ShouldPrint(map))
            {
                PrintPositions(map, sizeX, sizeY);
                Console.ReadKey();
            }
            for (var i = 0; i < positions.Count; i++)
            {
                var position = positions[i];
                var p = position.Item1 + position.Item2;
                positions[i] = new(new Vector2(
                        p.X >= 0 ? p.X % sizeX : (sizeX - (-p.X % sizeX)) % sizeX,
                        p.Y >= 0 ? p.Y % sizeY : (sizeY - (-p.Y % sizeY)) % sizeY),
                    position.Item2);
            }
            iterations++;
        }
    }

    private static readonly List<Vector2> Directions = [new(-1, 0), new(1, 0), new(0, 1), new(0, -1)];

    private static bool ShouldPrint(HashSet<Vector2> map)
    {
        foreach (var pos in map)
        {
            foreach (var direction in Directions)
            {
                var hasAll = true;
                for (int i = 1; i <= 10; i++)
                {
                    if (!map.Contains(pos + direction * i))
                    {
                        hasAll = false;
                        break;
                    }
                }
                if (hasAll) return true;
            }
        }
        return false;
    }

    private static void PrintPositions(HashSet<Vector2> map, int sizeX, int sizeY)
    {
        
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var element = map.Contains(new Vector2(x, y)) ? "#" : ".";
                Console.Write(element);
            }
            Console.WriteLine();
        }
    }
}