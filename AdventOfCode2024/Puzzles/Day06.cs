namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day06 : PuzzleBase
{
    private static (V2 position, V2 speed)? Next(Map map, V2 position, V2 speed)
    {
        var next = position + speed;
        return map.InBounds(next) ? map[next] == '.' ? (next, speed) : (position, speed.CW()) : null;
    }

    private static List<V2>? Solve1((V2 position, V2 speed) start, Map map)
    {
        var (position, speed) = start;
        var visited = new HashSet<(V2 Position, V2 speed)>();
        while (true)
        {
            if (!visited.Add((position, speed)))
            {
                return null;
            }

            var next = Next(map, position, speed);
            if (next == null)
            {
                break;
            }

            position = next.Value.position;
            speed = next.Value.speed;
        }

        return visited.Select(x => x.Position).Distinct().ToList();
    }

    public override void Solve()
    {
        var map = ReadMap();
        var start = (position: map.EnumeratePositions().Single(x => map[x] == '^'), speed: new V2(-1, 0));
        map[start.position] = '.';
        var path = Solve1(start, map);
        Console.WriteLine(path!.Count);
        var res2 = path.Select(x =>
        {
            if (map[x] == '^' || map[x] == '#')
            {
                return 0;
            }

            map[x] = '#';
            var result = Solve1(start, map) == null ? 1 : 0;
            map[x] = '.';
            return result;
        }).Sum();

        Console.WriteLine(res2);
    }
}
