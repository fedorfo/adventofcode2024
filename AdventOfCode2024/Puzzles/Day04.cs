namespace AdventOfCode2024.Puzzles;

using System.Text;
using Base;
using Helpers;

public class Day04 : PuzzleBase
{
    private static string? GetWord(Map map, V2 x, V2 v, int length = 4)
    {
        var res = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            if (!map.InBounds(x))
            {
                return null;
            }

            res.Append(map[x]);
            x += v;
        }

        return res.ToString();
    }

    private static bool IsMasSquare(Map map, V2 x)
    {
        var word1 = GetWord(map, x, new V2(1, 1), 3);
        var word2 = GetWord(map, x with { X = x.X + 2 }, new V2(-1, 1), 3);
        return word1 is "MAS" or "SAM" && word2 is "MAS" or "SAM";
    }

    public override void Solve()
    {
        var map = ReadMap();
        var cnt = map
            .EnumeratePositions()
            .Select(
                x => V2.Zero.GetNeighbours8().Select(v => GetWord(map, x, v) == "XMAS" ? 1 : 0).Sum()
            ).Sum();

        Console.WriteLine(cnt);

        var cnt2 = map.EnumeratePositions().Count(x => IsMasSquare(map, x));
        Console.WriteLine(cnt2);
    }
}
