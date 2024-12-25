namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day25 : PuzzleBase
{
    private static bool IsLock(Map schema) =>
        V2.EnumerateRange(V2.Zero, schema.Size with { X = 1 }).All(x => schema[x] == '#');

    private static bool IsKey(Map schema) =>
        V2.EnumerateRange(new V2(schema.Size.X - 1, 0), schema.Size).All(x => schema[x] == '#');


    private static List<int> LockPattern(Map @lock)
    {
        var result = new List<int>();
        for (var j = 0; j < @lock.Size.Y; j++)
        {
            for (var i = 0; i < @lock.Size.X; i++)
            {
                if (@lock[new V2(i, j)] != '#')
                {
                    result.Add(i - 1);
                    break;
                }
            }
        }

        return result;
    }

    private static List<int> KeyPattern(Map key)
    {
        var result = new List<int>();
        for (var j = 0; j < key.Size.Y; j++)
        {
            for (var i = key.Size.X - 1; i >= 0; i--)
            {
                if (key[new V2(i, j)] != '#')
                {
                    result.Add(key.Size.X - i - 2);
                    break;
                }
            }
        }

        return result;
    }

    public override void Solve()
    {
        var schemas = new List<Map>();
        while (true)
        {
            var map = ReadMap();
            if (map.Size.X == 0)
            {
                break;
            }

            schemas.Add(map);
        }

        var locks = schemas.Where(IsLock).ToArray();
        var keys = schemas.Where(IsKey).ToArray();
        var lockPattens = locks.Select(LockPattern).ToArray();
        var keyPatterns = keys.Select(KeyPattern).ToArray();
        var count = lockPattens.Sum(
            lockPattern =>
                keyPatterns.Count(
                    keyPattern => keyPattern.Zip(lockPattern, (a, b) => a + b <= 5).All(x => x)
                )
        );
        Console.WriteLine(count);
    }
}
