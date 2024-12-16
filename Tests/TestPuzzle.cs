namespace Tests;

using AdventOfCode2024.Base;
using NUnit.Framework;

[TestFixture]
public class TestPuzzle
{
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    [TestCase(16)]
    public void Test(int day)
    {
        const string baseDir = "../../../../AdventOfCode2024/bin/Debug/net9.0/";
        using var inputReader = new StringReader(File.ReadAllText($"{baseDir}input/{day:00}.txt"));
        Console.SetIn(inputReader);
        using var outputWriter = new StringWriter();
        Console.SetOut(outputWriter);

        var puzzleType = typeof(IPuzzle).Assembly.GetExportedTypes()
            .Single(x => x.Namespace == "AdventOfCode2024.Puzzles" && x.Name == $"Day{day:00}")!;
        var puzzle = (IPuzzle)Activator.CreateInstance(puzzleType)!;
        puzzle.Solve();
        Assert.That(
            outputWriter.ToString().Trim(),
            Is.EqualTo(File.ReadAllText($"{baseDir}/output/{day:00}.txt").Trim()),
            "The program output does not match the expected output."
        );
    }
}
