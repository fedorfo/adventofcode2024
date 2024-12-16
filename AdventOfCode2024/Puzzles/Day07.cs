namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day07 : PuzzleBase
{
    private static long Sum(long a, long b) => a + b;
    private static long Mult(long a, long b) => a * b;
    private static long Concat(long a, long b) => long.Parse(a + b.ToString());

    private static long SolveEquation(long result, long[] args, Func<long, long, long>[] operands, int step = 0,
        long? subResult = null)
    {
        if (subResult == null)
        {
            return SolveEquation(result, args, operands, step + 1, args[step]);
        }

        if (subResult > result)
        {
            return 0;
        }

        if (step == args.Length)
        {
            return subResult == result ? result : 0;
        }

        return operands
            .Select(x => SolveEquation(result, args, operands, step + 1, x(subResult.Value, args[step])))
            .Max();
    }

    public override void Solve()
    {
        var equations = ReadBlockLines().Select(x => x.ExtractTokens(' ', ':').Select(long.Parse).ToArray()).ToArray();
        Console.WriteLine(equations
            .Select(x => SolveEquation(x.First(), x.Skip(1).ToArray(), [Sum, Mult])).Sum()
        );
        Console.WriteLine(equations
            .Select(x => SolveEquation(x.First(), x.Skip(1).ToArray(), [Sum, Mult, Concat])).Sum()
        );
    }
}
