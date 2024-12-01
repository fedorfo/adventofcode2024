namespace AdventOfCode2024.Puzzles;

using Base;

public class Day01 : PuzzleBase
{
    public override void Solve() => Console.WriteLine(ReadLines().Single().Split(' ').Select(int.Parse).Sum());
}
