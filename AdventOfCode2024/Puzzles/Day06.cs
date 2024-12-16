namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day06 : PuzzleBase
{
    private static (V2 position, V2 speed) GetInitialPosition(char[][] map)
    {
        for (var i = 0; i < map.Length; i++)
        for (var j = 0; j < map[0].Length; j++)
        {
            if (map[i][j] == '^')
            {
                return (new V2(i, j), new V2(-1, 0));
            }
        }

        throw new InvalidOperationException("No initial position found");
    }

    private static (V2 position, V2 speed)? Next(char[][] map, V2 position, V2 speed)
    {
        var next = position + speed;
        if (!(next >= V2.Zero && next < new V2(map.Length, map[0].Length)))
        {
            return null;
        }

        if (map[next.X][next.Y] == '.')
        {
            return (next, speed);
        }

        return (position, new V2(speed.Y, -speed.X));
    }

    private static int Solve1(char[][] map)
    {
        var (position, speed) = GetInitialPosition(map);
        map[position.X][position.Y] = '.';
        var visited = new HashSet<(V2 Position, V2 speed)>();
        while (true)
        {
            if (!visited.Add((position, speed)))
            {
                return -1;
            }

            var next = Next(map, position, speed);
            if (next == null)
            {
                break;
            }

            position = next.Value.position;
            speed = next.Value.speed;
        }

        return visited.Select(x => x.Position).Distinct().Count();
    }

    public override void Solve()
    {
        var input = ReadBlockLines().ToList();
        Console.WriteLine(Solve1(input.Select(x => x.ToArray()).ToArray()));
        var res2 = 0;
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == '^')
                {
                    continue;
                }

                var map = input.Select(x => x.ToArray()).ToArray();
                map[i][j] = '#';
                if (Solve1(map) == -1)
                {
                    res2++;
                }
            }
        }

        Console.WriteLine(res2);
    }
}
