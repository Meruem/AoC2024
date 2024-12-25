using System.Diagnostics;
using AoC2024.Solution;
using AoC2024.Utils;

var sw = new Stopwatch();
// var solutions = new List<SolutionBase>
// {
//     //new Day06(), // 2100
//     //new Day09(), // 350
//     //new Day18(),  // 740
//     //new Day22(),  // 480
// };
var solutions = new List<SolutionBase>
{
    new Day01(), new Day02(), new Day03(), new Day04(), new Day05(), new Day06(), new Day07(), new Day08(), new Day09(),
    new Day10(), new Day11(), new Day12(), new Day13(), new Day14(), new Day15(), new Day16(), new Day17(), new Day18(),
    new Day19(), new Day20(), new Day21(), new Day22(), new Day23(), new Day24(), new Day25()
};


sw.Start();
long elapsedMilliseconds = 0;
foreach (var solution in solutions)
{
    var part1 = solution.Part1();
    sw.Stop();
    Console.WriteLine($"{solution.Name} Part1: {part1} in {sw.ElapsedMilliseconds - elapsedMilliseconds} ms ");
    elapsedMilliseconds = sw.ElapsedMilliseconds;
    sw.Start();
    var part2 = solution.Part2();
    sw.Stop();
    Console.WriteLine($"{solution.Name} Part2: {part2} in {sw.ElapsedMilliseconds - elapsedMilliseconds} ms ");
    elapsedMilliseconds = sw.ElapsedMilliseconds;
    sw.Start();
}

Console.WriteLine($"Total: {elapsedMilliseconds} ms");