namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day12 : PuzzleBase
{
    private List<V2> Bfs(Dictionary<V2, bool> visited, Map map, V2 start)
    {
        var queue = new Queue<V2>();
        queue.Enqueue(start);
        visited[start] = true;
        var result = new List<V2> { start };
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var neighbour in current.GetNeighbours4())
            {
                if (
                    map.InBounds(neighbour)
                    && !visited.GetValueOrDefault(neighbour, false)
                    && map[neighbour] == map[start]
                )
                {
                    visited[neighbour] = true;
                    queue.Enqueue(neighbour);
                    result.Add(neighbour);
                }
            }
        }

        return result;
    }

    private int GetScore(List<V2> area, Map map)
    {
        var square = area.Count;
        var perimeter = area.Select(x => x.GetNeighbours4().Count(y => !map.InBounds(y) || map[y] != map[x])).Sum();
        return square * perimeter;
    }

    private int GetScoreV2(List<V2> area, Map map)
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
        var visited = new Dictionary<V2, bool>();
        var areas = new List<List<V2>>();
        foreach (var v in map.EnumeratePositions())
        {
            if (!visited.GetValueOrDefault(v, false))
            {
                areas.Add(this.Bfs(visited, map, v));
            }
        }

        Console.WriteLine(areas.Select(x => this.GetScore(x, map)).Sum());
        Console.WriteLine(areas.Select(x => this.GetScoreV2(x, map)).Sum());
    }
}
