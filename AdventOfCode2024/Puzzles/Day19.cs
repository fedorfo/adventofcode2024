namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day19 : PuzzleBase
{
    private static long Dp(string design, string[] patterns)
    {
        var dp = new long[design.Length + 1];
        dp[0] = 1;
        for (var i = 0; i < design.Length; i++)
        {
            foreach (var pattern in patterns)
            {
                if (design.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    dp[i + pattern.Length] += dp[i];
                }
            }
        }

        return dp[design.Length];
    }

    public override void Solve()
    {
        var patterns = ReadBlockLines().Single().ExtractTokens(' ', ',').ToArray();
        var designs = ReadBlockLines().ToArray();
        var dp = designs.Select(x => Dp(x, patterns)).ToArray();
        Console.WriteLine(dp.Count(x => x > 0));
        Console.WriteLine(dp.Sum());
    }
}
