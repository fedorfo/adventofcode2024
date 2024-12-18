namespace AdventOfCode2024.Puzzles;

public class Day01 : PuzzleBase
{
    public override void Solve()
    {
        var pairs = ReadBlockLines().Select(x => x.Split("   ").ToArray()).ToArray();
        var left = pairs.Select(x => int.Parse(x[0])).Order().ToArray();
        var right = pairs.Select(x => int.Parse(x[1])).Order().ToArray();
        Console.WriteLine(left.Zip(right, (a, b) => Math.Abs(a - b)).Sum());

        var dict = right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        Console.WriteLine(left.Select(x => (long)dict.GetValueOrDefault(x, 0) * x).Sum());
    }
}
