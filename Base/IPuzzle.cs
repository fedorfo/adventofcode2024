namespace AdventOfCode2024.Base;

public interface IPuzzle
{
    int Day { get; }
    string InputFileName { get; }
    void Solve();
}
