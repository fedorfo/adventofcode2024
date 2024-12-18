namespace AdventOfCode2024.Puzzles;

public interface IPuzzle
{
    int Day { get; }
    string InputFileName { get; }
    void Solve();
}
