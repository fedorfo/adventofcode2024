namespace AdventOfCode2024.Puzzles;

using Base;

public class Day02 : PuzzleBase
{
    private bool IsSafe(int[] report) =>
        report.Zip(report.Skip(1), (a, b) => Math.Abs(a - b)).All(x => x is >= 1 and <= 3) &&
        (
            report.Zip(report.Skip(1), (a, b) => a <= b).All(x => x) ||
            report.Zip(report.Skip(1), (a, b) => a >= b).All(x => x)
        );

    private bool IsSafeV2(int[] report) =>
        Enumerable.Range(0, report.Length).Any(i => this.IsSafe(report.Take(i).Concat(report.Skip(i + 1)).ToArray()));

    public override void Solve()
    {
        var reports = ReadLines().Select(x => x.Split().Select(int.Parse).ToArray()).ToArray();
        Console.WriteLine(reports.Count(this.IsSafe));
        Console.WriteLine(reports.Count(this.IsSafeV2));
    }
}
