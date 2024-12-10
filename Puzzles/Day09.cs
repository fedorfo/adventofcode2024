namespace AdventOfCode2024.Puzzles;

using Base;

public class Day09 : PuzzleBase
{
    private static (int[] fiels, int[] spaces) PrepareFilesAndSpaces(int[] diskMap) => (
        diskMap.Where((_, i) => i % 2 == 0).ToArray(), diskMap.Where((_, i) => i % 2 == 1).ToArray());

    private static void Solve1(int[] diskMap)
    {
        var (files, spaces) = PrepareFilesAndSpaces(diskMap);
        var current = files.Length - 1;
        var result = new List<int>();
        for (var i = 0; i < files.Length; i++)
        {
            result.AddRange(Enumerable.Repeat(i, files[i]));

            if (i >= spaces.Length)
            {
                continue;
            }

            for (var j = 0; j < spaces[i]; j++)
            {
                if (current <= i)
                {
                    break;
                }

                if (files[current] == 0)
                {
                    current--;
                }

                if (current <= i)
                {
                    break;
                }

                files[current]--;
                result.Add(current);
            }
        }

        Console.WriteLine(result.Select((x, i) => (long)x * i).Sum());
    }

    private static void Solve2(int[] diskMap)
    {
        var (files, spaces) = PrepareFilesAndSpaces(diskMap);

        var spaceOccupiedByFiles = spaces.Select(_ => new List<int>()).ToArray();
        var fileMoved = files.Select(_ => false).ToArray();
        for (var i = files.Length - 1; i >= 0; i--)
        {
            for (var j = 0; j < i; j++)
            {
                if (spaces.Length > j && spaces[j] >= files[i])
                {
                    spaces[j] -= files[i];
                    spaceOccupiedByFiles[j].Add(i);
                    fileMoved[i] = true;
                    break;
                }
            }
        }

        var result = new List<int>();
        for (var i = 0; i < files.Length; i++)
        {
            result.AddRange(Enumerable.Repeat(fileMoved[i] ? 0 : i, files[i]));

            if (i < spaceOccupiedByFiles.Length)
            {
                spaceOccupiedByFiles[i].ForEach(fileId => result.AddRange(Enumerable.Repeat(fileId, files[fileId])));
                result.AddRange(Enumerable.Repeat(0, spaces[i]));
            }
        }
        Console.WriteLine(result.Select((x, i) => (long)x * i).Sum());
    }

    public override void Solve()
    {
        var diskMap = ReadAll().Select(x => x - '0').ToArray();
        Solve1(diskMap);
        Solve2(diskMap);
    }
}
