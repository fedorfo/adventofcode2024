namespace AdventOfCode2024.Puzzles;

using Helpers;
using MoreLinq;

public class Day21 : PuzzleBase
{
    private static readonly Map NumericKeypad = new(["789", "456", "123", " 0A"]);

    private static readonly Map RemoteControlKeypad = new([" ^A", "<v>"]);

    private static readonly Dictionary<V2, string> Directions = new()
    {
        [new V2(-1, 0)] = "^", [new V2(1, 0)] = "v", [new V2(0, -1)] = "<", [new V2(0, 1)] = ">"
    };

    private static IEnumerable<string> Concat(IEnumerable<string> a, IEnumerable<string> b)
    {
        foreach (var x in a)
        foreach (var y in b)
        {
            yield return $"{x}{y}";
        }
    }

    private static string[] GetWays(Map keypad, char keyFrom, char keyTo)
    {
        if (keyFrom == keyTo)
        {
            return [""];
        }

        var start = keypad.EnumeratePositions().Single(x => keypad[x] == keyFrom);
        var end = keypad.EnumeratePositions().Single(x => keypad[x] == keyTo);
        var result = new List<string>();
        foreach (var next in start.GetNeighbours4().Where(x => keypad.InBounds(x) && keypad[x] != ' ')
                     .Where(x => (end - x).ManhattanLength() < (end - start).ManhattanLength()))
        {
            var ways = GetWays(keypad, keypad[next], keypad[end]);
            result.AddRange(Concat([Directions[next - start]], ways));
        }

        return result.ToArray();
    }

    private static string[] GetWays(Map keypad, string s)
    {
        string[] result = [""];
        var curKey = 'A';
        foreach (var key in s)
        {
            result = Concat(result, GetWays(keypad, curKey, key)).ToArray();
            result = Concat(result, ["A"]).ToArray();
            curKey = key;
        }

        return result;
    }


    private static int Complexity(string code)
    {
        Console.WriteLine($"Start complexity {code}");
        var ways = GetWays(NumericKeypad, code);
        ways = ways.SelectMany(w=>GetWays(RemoteControlKeypad, w)).Distinct().ToArray();
        ways = ways.SelectMany(w=>GetWays(RemoteControlKeypad, w)).Distinct().ToArray();
        //ways = ways.SelectMany(w=>GetWays(RemoteControlKeypad, w)).Distinct().ToArray();

        var shortest = ways.Select(x => x.Length).Min();
        return shortest * int.Parse(new string(code.Where(char.IsDigit).ToArray()));
    }

    public override void Solve()
    {
        var codes = ReadBlockLines().ToArray();
        Console.WriteLine(codes.Select(Complexity).Sum());
    }
}
