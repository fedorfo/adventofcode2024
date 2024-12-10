namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;
using MoreLinq.Extensions;

public class Day10 : PuzzleBase
{
    private int ScoreV1(char[][] map, V2 p)
    {
        var mapSize = new V2(map.Length, map[0].Length);
        if (map[p.X][p.Y] != '0')
        {
            return 0;
        }

        var queue = new Queue<V2>();
        queue.Enqueue(p);
        var visited = new HashSet<V2> { p };
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            foreach (var u in v.GetNeighbours4().Where(x => V2.Zero <= x && x < mapSize))
            {
                if (map[u.X][u.Y] - map[v.X][v.Y] == 1 && visited.Add(u))
                {
                    queue.Enqueue(u);
                }
            }
        }

        return V2.EnumerateRange(V2.Zero, mapSize).Count(v => visited.Contains(v) && map[v.X][v.Y] == '9');
    }

    private int ScoreV2(char[][] map, V2 p)
    {
        var mapSize = new V2(map.Length, map[0].Length);
        var dp = map.Select(x => x.Select(_ => 0).ToArray()).ToArray();
        dp[p.X][p.Y] = 1;
        for (var i = 1; i <= 9; i++)
        {
            V2.EnumerateRange(V2.Zero, mapSize).Where(x => map[x.X][x.Y] == '0' + i).ForEach(v =>
            {
                dp[v.X][v.Y] = v.GetNeighbours4()
                    .Where(x => V2.Zero <= x && x < mapSize && map[x.X][x.Y] == '0' + i - 1)
                    .Sum(x => dp[x.X][x.Y]);
            });
        }

        return V2.EnumerateRange(V2.Zero, mapSize).Select(x => map[x.X][x.Y] == '9' ? dp[x.X][x.Y] : 0).Sum();
    }

    public override void Solve()
    {
        var map = ReadMap();
        var mapSize = new V2(map.Length, map[0].Length);
        Console.WriteLine(V2.EnumerateRange(V2.Zero, mapSize).Select(p => this.ScoreV1(map, p)).Sum());
        Console.WriteLine(V2.EnumerateRange(V2.Zero, mapSize).Select(p => this.ScoreV2(map, p)).Sum());
    }
}
