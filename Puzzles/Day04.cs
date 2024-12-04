namespace AdventOfCode2024.Puzzles;

using System.Text;
using Base;

public class Day04 : PuzzleBase
{
    private static string? GetWord(string[] field, int x, int y, int vx, int vy, int length = 4)
    {
        var res = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            if (x < 0 || x >= field.Length || y < 0 || y >= field[0].Length)
            {
                return null;
            }

            res.Append(field[x][y]);
            x += vx;
            y += vy;
        }

        return res.ToString();
    }

    private static bool IsMasSquare(string[] field, int x, int y)
    {
        var word1 = GetWord(field, x, y, 1, 1, 3);
        var word2 = GetWord(field, x + 2, y, -1, 1, 3);
        return word1 is "MAS" or "SAM" && word2 is "MAS" or "SAM";
    }

    public override void Solve()
    {
        var field = ReadLines().ToArray();
        var cnt = 0;
        for (var x = 0; x < field.Length; x++)
        for (var y = 0; y < field[x].Length; y++)
        for (var vx = -1; vx <= 1; vx++)
        for (var vy = -1; vy <= 1; vy++)
        {
            if (vx == 0 && vy == 0)
            {
                continue;
            }

            if (GetWord(field, x, y, vx, vy) == "XMAS")
            {
                cnt++;
            }
        }

        Console.WriteLine(cnt);

        var cnt2 = 0;
        for (var x = 0; x < field.Length; x++)
        for (var y = 0; y < field[x].Length; y++)
        {
            if (IsMasSquare(field, x, y))
            {
                cnt2++;
            }
        }

        Console.WriteLine(cnt2);
    }
}
