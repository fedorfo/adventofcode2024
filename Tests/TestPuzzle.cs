namespace Tests;

using AdventOfCode2024.Puzzles;
using AdventOfCode2024.Resources;
using NUnit.Framework;

[TestFixture]
public class TestPuzzle
{
    public static IEnumerable<TestCaseData> GetTestCases =>
        PuzzleRegistry.GetPuzzles().Select(
            x => new TestCaseData(x.Day).SetName($"Day{x.Day:00}").SetCategory("DynamicTest")
        );

    [TestCaseSource(nameof(GetTestCases))]
    public void Test(int day)
    {
        using var inputReader = new StreamReader(ResourceRegistry.GetResourceStream($"{day:00}.in"));
        Console.SetIn(inputReader);
        using var outputWriter = new StringWriter();
        Console.SetOut(outputWriter);

        var puzzle = PuzzleRegistry.GetPuzzles().Single(x => x.Day == day);
        puzzle.Solve();
        using var outputReader = new StreamReader(ResourceRegistry.GetResourceStream($"{day:00}.out"));
        var expectedOutput = outputReader.ReadToEnd();
        Assert.That(
            outputWriter.ToString().Trim(),
            Is.EqualTo(expectedOutput.Trim()),
            "The program output does not match the expected output."
        );
    }
}
