namespace AdventOfCode2024.Puzzles;

using Helpers;
using MoreLinq.Extensions;

public class Day18 : PuzzleBase
{
    public override void Solve()
    {
        var bytes = ReadBlockLines()
            .Select(x => x.ExtractTokens(',').Select(int.Parse).ToArray())
            .Select(x => new V2(x[0], x[1]))
            .ToArray();

        Console.WriteLine(DistanceToExit(bytes.Take(1024)));
        var result = bytes[Helpers.BinarySearch(bytes, i => DistanceToExit(bytes.Take(i)) == -1) - 1];
        Console.WriteLine($"{result.X},{result.Y}");
    }

    private static long DistanceToExit(IEnumerable<V2> bytes)
    {
        var map = new Map(71, 71);
        bytes.ForEach(x => map[x] = '#');
        var result = GraphAlgo.Bfs<V2>(
                [V2.Zero],
                v => v.GetNeighbours4().Where(x => map.InBounds(x) && map[x] != '#')
            )
            .SingleOrDefault(x => x.Vertex == map.Size - new V2(1, 1));
        return result?.Distance ?? -1;
    }
}