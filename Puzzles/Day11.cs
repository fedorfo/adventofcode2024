namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;
using ForEachExtension = MoreLinq.Extensions.ForEachExtension;

public class Day11 : PuzzleBase
{
    private static IEnumerable<long> Blink(long stone)
    {
        if (stone == 0)
        {
            yield return 1;
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            var stoneStr = stone.ToString();
            yield return long.Parse(stoneStr.Substring(0, stoneStr.Length / 2));
            yield return long.Parse(stoneStr.Substring(stoneStr.Length / 2, stoneStr.Length / 2));
        }
        else
        {
            yield return stone * 2024;
        }
    }

    private static Dictionary<long, long> Blink(Dictionary<long, long> stonesDict, Dictionary<long, List<long>> map)
    {
        var result = stonesDict.Keys.ToDictionary(x => x, _ => (long)0);
        foreach (var stone in stonesDict.Keys)
        {
            map[stone].ForEach(newStone => result[newStone] += stonesDict[stone]);
        }

        return result;
    }

    private static void ExtendMap(Dictionary<long, List<long>> map, long stone)
    {
        if (!map.ContainsKey(stone))
        {
            var newStones = Blink(stone).ToList();
            map[stone] = newStones;
            newStones.ForEach(x => ExtendMap(map, x));
        }
    }

    public override void Solve()
    {
        var stones = ReadLines().Single().ExtractTokens().Select(long.Parse).ToList();
        var map = new Dictionary<long, List<long>>();
        stones.ForEach(x => ExtendMap(map, x));
        var stonesDict = map.Keys.ToDictionary(x => x, _ => (long)0);
        stones.ForEach(stone => stonesDict[stone]++);
        ForEachExtension.ForEach(Enumerable.Range(0, 25), _ => stonesDict = Blink(stonesDict, map));
        Console.WriteLine(stonesDict.Values.Sum());
        ForEachExtension.ForEach(Enumerable.Range(0, 50), _ => stonesDict = Blink(stonesDict, map));
        Console.WriteLine(stonesDict.Values.Sum());
    }
}
