namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;
using MoreLinq.Extensions;

public class Day15 : PuzzleBase
{
    private static void Move(Map map, List<V2> positions, V2 v)
    {
        var allNextPositions = positions.Select(x => x + v).ToList();
        if (allNextPositions.Any(x => map[x] == '#'))
        {
            return;
        }

        var boxes = allNextPositions.Where(x => "[]O".Contains(map[x])).ToList();
        boxes = boxes.Select(
            x =>
            {
                if (v.X != 0 && map[x] == '[')
                {
                    return x + new V2(0, 1);
                }

                if (v.X != 0 && map[x] == ']')
                {
                    return x + new V2(0, -1);
                }

                return null;
            }).Where(x => x != null).Concat(boxes).Distinct().ToList()!;
        if (boxes.Count > 0)
        {
            Move(map, boxes, v);
        }

        if (allNextPositions.All(x => map[x] == '.'))
        {
            positions.ForEach(x =>
            {
                map[x + v] = map[x];
                map[x] = '.';
            });
        }
    }

    private static void ExecuteCommand(Map map, char command)
    {
        var robot = map.EnumeratePositions().Single(x => map[x] == '@');
        var v = command switch
        {
            '^' => new V2(-1, 0),
            '>' => new V2(0, 1),
            'v' => new V2(1, 0),
            '<' => new V2(0, -1),
            _ => throw new InvalidOperationException()
        };
        Move(map, [robot], v);
    }

    public override void Solve()
    {
        var map = ReadMap();
        var map1 = map.Copy();
        var commands = ReadBlockText().Replace("\n", "");
        commands.ForEach(x => ExecuteCommand(map1, x));
        Console.WriteLine(map1.EnumeratePositions().Where(x => map[x] == 'O').Sum(x => (x.X * 100) + x.Y));

        var map2 = new Map(map.GetMap().Select(x => string.Join("", x.Select(y => y switch
        {
            'O' => "[]",
            '.' => "..",
            '#' => "##",
            '@' => "@.",
            _ => throw new ArgumentOutOfRangeException(nameof(y), y, null)
        })).ToArray()).ToArray());
        commands.ForEach(x => ExecuteCommand(map2, x));
        Console.WriteLine(map2.EnumeratePositions().Where(x => map2[x] == '[').Sum(x => (x.X * 100) + x.Y));
    }
}
