namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day20 : PuzzleBase
{
    private static IEnumerable<(V2 End, int Dist)> GetCheats(Map map, V2 start, int cheatLength) =>
        V2
            .EnumerateRange(new V2(-cheatLength, -cheatLength), new V2(cheatLength + 1, cheatLength + 1))
            .Select(x => x + start)
            .Where(x => map.InBounds(x) && map[x] == '.')
            .Where(x => (x - start).ManhattanLength() <= cheatLength)
            .Select(x => (x, (x - start).ManhattanLength()));


    public override void Solve()
    {
        var map = ReadMap();
        var start = map.EnumeratePositions().Single(x => map[x] == 'S');
        var end = map.EnumeratePositions().Single(x => map[x] == 'E');
        map[start] = map[end] = '.';
        var distance = GraphAlgo.Bfs([start],
            v => v.GetNeighbours4().Where(u => map.InBounds(u) && map[u] == '.')
        ).Single(x => x.Vertex == end).Path().Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
        var vertices = distance.Keys.ToArray();
        var ans1 = vertices
            .Select(x => GetCheats(map, x, 2).Count(cheat => distance[x] + cheat.Dist + 100 <= distance[cheat.End]))
            .Sum();
        Console.WriteLine(ans1);
        var ans2 = vertices
            .Select(x => GetCheats(map, x, 20).Count(cheat => distance[x] + cheat.Dist + 100 <= distance[cheat.End]))
            .Sum();
        Console.WriteLine(ans2);
    }
}
