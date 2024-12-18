namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day16 : PuzzleBase
{
    public override void Solve()
    {
        var map = ReadMap();
        var start = map.EnumeratePositions().Single(x => map[x] == 'S');
        var end = map.EnumeratePositions().Single(x => map[x] == 'E');
        map[start] = '.';
        map[end] = '.';
        var walker = new MapWalker(map);
        var path = GraphAlgo.Dijikstra([(Vertex: start, Direction: new V2(0, 1))], walker.Next).ToArray();
        var result = path.First(x => x.Vertex.Vertex == end);
        Console.WriteLine(result.Distance);

        var pathDict = path.ToDictionary(x => x.Vertex, x => x.Distance);
        var ends = path.Where(x => x.Vertex.Vertex == end && x.Distance == result.Distance).ToArray();
        var walker2 = new MapWalker2(map, pathDict);
        var result2 = GraphAlgo.Bfs(ends.Select(x => x.Vertex), walker2.Prev).Select(x => x.Vertex.Vertex).Distinct()
            .ToArray();
        Console.WriteLine(result2.Length);
    }

    private sealed class MapWalker(Map map)
    {
        public IEnumerable<((V2 Vertex, V2 Direction), long Distance)> Next((V2 Vertex, V2 Direction) x)
        {
            yield return ((x.Vertex, x.Direction.CW()), 1000);
            yield return ((x.Vertex, x.Direction.CCW()), 1000);
            if (map.InBounds(x.Vertex + x.Direction) && map[x.Vertex + x.Direction] == '.')
            {
                yield return ((x.Vertex + x.Direction, x.Direction), 1);
            }
        }
    }

    private sealed class MapWalker2(Map map, Dictionary<(V2 Vertex, V2 Direction), long> pathDict)
    {
        private IEnumerable<((V2 Vertex, V2 Direction), long Distance)> TryPrev((V2 Vertex, V2 Direction) x)
        {
            yield return ((x.Vertex, x.Direction.CW()), -1000);
            yield return ((x.Vertex, x.Direction.CCW()), -1000);
            if (map.InBounds(x.Vertex - x.Direction) && map[x.Vertex - x.Direction] == '.')
            {
                yield return ((x.Vertex - x.Direction, x.Direction), -1);
            }
        }

        public IEnumerable<(V2 Vertex, V2 Direction)> Prev((V2 Vertex, V2 Direction) x)
        {
            foreach (var next in this.TryPrev(x))
            {
                if (pathDict.TryGetValue(next.Item1, out var distance) && distance == pathDict[x] + next.Item2)
                {
                    yield return next.Item1;
                }
            }
        }
    }
}
