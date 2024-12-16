namespace AdventOfCode2024.Puzzles;

using System.Text.RegularExpressions;
using Base;
using Helpers;

public class Day13 : PuzzleBase
{
    private static readonly Regex ButtonRegex = new(@"Button [AB]: X\+(\d+), Y\+(\d+)");

    private static readonly Regex PrizeRegex = new(@"Prize: X=(\d+), Y=(\d+)");

    private static V2 Parse(string line, Regex regex)
    {
        var match = regex.Match(line);
        return new V2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }

    private static ClawMachineConfig Parse(string[] lines) =>
        new(
            Parse(lines[0], ButtonRegex),
            Parse(lines[1], ButtonRegex),
            Parse(lines[2], PrizeRegex)
        );

    private static long Score(long ax, long ay, long bx, long by, long px, long py)
    {
        var solution = Helpers.SolveLinearSystem(new Fraction[,] { { ax, bx }, { ay, by } }, [px, py]);
        if (solution[0].IsInt() && solution[1].IsInt() && solution[0] >= 0 && solution[1] >= 0)
        {
            return (solution[0].ToLong() * 3) + solution[1].ToLong();
        }

        return 0;
    }

    public override void Solve()
    {
        var clawMachines = new List<ClawMachineConfig>();
        while (true)
        {
            var block = ReadBlockLines().ToList();
            if (block.Count == 0)
            {
                break;
            }

            clawMachines.Add(Parse(block.ToArray()));
        }

        Console.WriteLine(
            clawMachines.Select(x => Score(x.A.X, x.A.Y, x.B.X, x.B.Y, x.Prize.X, x.Prize.Y)).Sum());
        Console.WriteLine(
            clawMachines.Select(x =>
                Score(x.A.X, x.A.Y, x.B.X, x.B.Y, x.Prize.X + 10000000000000, x.Prize.Y + 10000000000000)).Sum()
        );
    }

    private sealed record ClawMachineConfig(V2 A, V2 B, V2 Prize);
}
