namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day12 : PuzzleBase
{
    private static int GetScore(List<V2> area, Map map)
    {
        var square = area.Count;
        var perimeter = area.Select(x => x.GetNeighbours4().Count(y => !map.InBounds(y) || map[y] != map[x])).Sum();
        return square * perimeter;
    }

    private static int GetScoreV2(List<V2> area, Map map)
    {
        var square = area.Count;
        var hs = new[] { new List<V2>(), new List<V2>() };
        var vs = new[] { new List<V2>(), new List<V2>() };
        foreach (var v in area)
        {
            foreach (var u in v.GetNeighbours4().Where(x => !map.InBounds(x) || map[x] != map[v]))
            {
                if (v.X == u.X)
                {
                    vs[v.Y < u.Y ? 0 : 1].Add(v with { Y = Math.Min(v.Y, u.Y) });
                }
                else
                {
                    hs[v.X < u.X ? 0 : 1].Add(v with { X = Math.Min(v.X, u.X) });
                }
            }
        }

        vs = vs.Select(s => s.OrderBy(x => x.Y).ThenBy(x => x.X).ToList()).ToArray();
        hs = hs.Select(s => s.OrderBy(x => x.X).ThenBy(x => x.Y).ToList()).ToArray();

        var sides = 4
                    + vs.Select(s => s.Zip(s.Skip(1), (v, u) => v.Y == u.Y && v.X + 1 == u.X ? 0 : 1).Sum()).Sum()
                    + hs.Select(s => s.Zip(s.Skip(1), (v, u) => v.X == u.X && v.Y + 1 == u.Y ? 0 : 1).Sum()).Sum();

        return square * sides;
    }

    public override void Solve()
    {
        var map = ReadMap();
        var visited = new HashSet<V2>();
        var areas = new List<List<V2>>();
        foreach (var v in map.EnumeratePositions())
        {
            if (!visited.Contains(v))
            {
                var area = GraphAlgo.Bfs(
                    [v],
                    x => x.GetNeighbours4().Where(y => map.InBounds(y) && map[x] == map[y])
                ).Select(x => x.Vertex).ToList();
                visited.UnionWith(area);
                areas.Add(area);
            }
        }

        Console.WriteLine(areas.Select(x => GetScore(x, map)).Sum());
        Console.WriteLine(areas.Select(x => GetScoreV2(x, map)).Sum());
    }
}
