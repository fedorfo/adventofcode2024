namespace AdventOfCode2024.Puzzles;

using System.Reflection;

public static class PuzzleRegistry
{
    private static List<TypeInfo> GetPuzzleTypeInfos() => typeof(Program).Assembly.DefinedTypes.Where(
        x => x.ImplementedInterfaces.Contains(typeof(IPuzzle)) && !x.IsAbstract
    ).ToList();

    public static List<IPuzzle> GetPuzzles() => GetPuzzleTypeInfos()
        .Select(puzzleTypeInfo => puzzleTypeInfo.GetConstructor([]))
        .Select(constructor => (IPuzzle)constructor!.Invoke([]))
        .ToList();
}
