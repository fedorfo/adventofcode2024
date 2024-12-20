namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day05 : PuzzleBase
{
    private const int Infinity = 1000000000;

    private static int[][] FilterOrders(int[][] orders, int[] update) =>
        orders.Where(x => update.Contains(x[0]) && update.Contains(x[1])).ToArray();

    private static int[][] PrepareDistance(int[][] orders, int[] update)
    {
        var vertices = orders.Select(x => x[0]).Concat(orders.Select(x => x[1])).Concat(update)
            .Distinct().Order().ToArray();
        var n = vertices.Max();

        var distance = Enumerable.Range(0, n + 1)
            .Select(_ => Enumerable.Range(0, n + 1).Select(_ => Infinity).ToArray())
            .ToArray();
        foreach (var order in orders)
        {
            distance[order[0]][order[1]] = 1;
        }

        for (var i = 0; i <= n; i++)
        {
            distance[i][i] = 0;
        }

        for (var k = 0; k <= n; k++)
        for (var i = 0; i <= n; i++)
        for (var j = 0; j <= n; j++)
        {
            if (distance[i][j] > distance[i][k] + distance[k][j])
            {
                distance[i][j] = distance[i][k] + distance[k][j];
            }
        }

        return distance;
    }

    private static bool IsCorrectUpdate(int[][] orders, int[] update)
    {
        orders = FilterOrders(orders, update);
        var distance = PrepareDistance(orders, update);

        for (var i = 0; i < update.Length; i++)
        for (var j = i + 1; j < update.Length; j++)
        {
            if (distance[update[j]][update[i]] != Infinity)
            {
                return false;
            }
        }

        return true;
    }

    private static int[] Correct(int[][] orders, int[] update)
    {
        orders = FilterOrders(orders, update);
        var distance = PrepareDistance(orders, update);
        return update.Order(new Comparer(distance)).ToArray();
    }

    public override void Solve()
    {
        var orders = ReadBlockLines().Select(x => x.ExtractTokens('|').Select(int.Parse).ToArray()).ToArray();
        var updates = ReadBlockLines().Select(x => x.ExtractTokens(',').Select(int.Parse).ToArray()).ToArray();

        var updatesIsCorrect = updates
            .Select(update => (Update: update, IsCorrect: IsCorrectUpdate(orders, update))).ToArray();
        Console.WriteLine(updatesIsCorrect.Where(x => x.IsCorrect).Select(x => x.Update).Select(x => x[x.Length / 2])
            .Sum());


        Console.WriteLine(updatesIsCorrect.Where(x => !x.IsCorrect).Select(x => x.Update)
            .Select(x => Correct(orders, x)).Select(x => x[x.Length / 2]).Sum());
    }

    private sealed class Comparer(int[][] distance) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (distance[x][y] == 0)
            {
                return 0;
            }

            if (distance[x][y] != Infinity)
            {
                return -1;
            }

            if (distance[y][x] != Infinity)
            {
                return 1;
            }

            throw new Exception("Invalid input");
        }
    }
}
