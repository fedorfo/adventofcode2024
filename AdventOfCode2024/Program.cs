namespace AdventOfCode2024;

using System.Diagnostics;
using Puzzles;
using Resources;

public class Program
{
    public static void Main()
    {
        var puzzles = PuzzleRegistry.GetPuzzles().ToDictionary(puzzle => puzzle.Day);
        var day = Helpers.Helpers.Max(puzzles.Keys.ToArray());
        using var inputStream = new StreamReader(ResourceRegistry.GetResourceStream(puzzles[day].InputFileName));
        Console.SetIn(inputStream);
        var sw = Stopwatch.StartNew();
        puzzles[day].Solve();
        Console.WriteLine("Elapsed: " + sw.Elapsed);
    }
}
