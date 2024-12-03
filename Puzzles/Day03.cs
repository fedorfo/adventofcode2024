namespace AdventOfCode2024.Puzzles;

using System.Text.RegularExpressions;
using Base;

public class Day03 : PuzzleBase
{
    private static readonly Regex RegexMul = new(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
    private static readonly Regex RegexDo = new(@"do\(\)", RegexOptions.Compiled);
    private static readonly Regex RegexDont = new(@"don't\(\)", RegexOptions.Compiled);

    private static bool IsEnabled(int index, (int Index, bool IsEnabled)[] switchers) =>
        switchers.Last(x => x.Index < index).IsEnabled;

    public override void Solve()
    {
        var input = ReadAll();
        var muls = RegexMul.Matches(input)
            .Select(x => (Value: int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value), x.Index)).ToArray();
        var switchers = new[] { (-1, true) }
            .Concat(RegexDo.Matches(input).Select(x => (x.Index, true)))
            .Concat(RegexDont.Matches(input).Select(x => (x.Index, false)))
            .OrderBy(x => x.Item1).ToArray();

        Console.WriteLine(muls.Sum(x => x.Value));
        Console.WriteLine(muls.Where(x => IsEnabled(x.Index, switchers)).Sum(x => x.Value));
    }
}
