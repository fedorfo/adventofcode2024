namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day07 : PuzzleBase
{
    private long Sum(long a, long b) => a + b;
    private long Mult(long a, long b) => a * b;
    private long Concat(long a, long b) => long.Parse(a + b.ToString());

    private long SolveEquation(long result, long[] args, Func<long, long, long>[] operands, int step = 0,
        long? subResult = null)
    {
        if (subResult == null)
        {
            return this.SolveEquation(result, args, operands, step + 1, args[step]);
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
            .Select(x => this.SolveEquation(result, args, operands, step + 1, x(subResult.Value, args[step])))
            .Max();
    }

    public override void Solve()
    {
        var input = ReadLines();
        var equations = input.Select(x => x.ExtractTokens(' ', ':').Select(long.Parse).ToArray()).ToArray();
        Console.WriteLine(equations
            .Select(x => this.SolveEquation(x.First(), x.Skip(1).ToArray(), [this.Sum, this.Mult])).Sum()
        );
        Console.WriteLine(equations
            .Select(x => this.SolveEquation(x.First(), x.Skip(1).ToArray(), [this.Sum, this.Mult, this.Concat])).Sum()
        );
    }
}
