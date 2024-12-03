namespace AdventOfCode2024.Puzzles;

using Base;

public class Day02 : PuzzleBase
{
    private bool IsSafe(int[] report)
    {
        if (report.Zip(report.Skip(1), (a, b) => Math.Abs(a - b)).Any(x => x is < 1 or > 3))
        {
            return false;
        }

        return report.Zip(report.Skip(1), (a, b) => a <= b).All(x => x) ||
               report.Zip(report.Skip(1), (a, b) => a >= b).All(x => x);
    }

    private bool IsSafeV2(int[] report)
    {
        for (var i = 0; i < report.Length; i++)
        {
            var candidate = report.Take(i).Concat(report.Skip(i + 1)).ToArray();
            if (this.IsSafe(candidate))
            {
                return true;
            }
        }

        return false;
    }

    public override void Solve()
    {
        var reports = ReadLines().Select(x => x.Split().Select(int.Parse).ToArray()).ToArray();
        Console.WriteLine(reports.Count(this.IsSafe));
        Console.WriteLine(reports.Count(this.IsSafeV2));
    }
}
