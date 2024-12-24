namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day21 : PuzzleBase
{
    private static readonly Dictionary<V2, string> Directions = new()
    {
        [new V2(-1, 0)] = "^", [new V2(1, 0)] = "v", [new V2(0, -1)] = "<", [new V2(0, 1)] = ">"
    };

    private static readonly Map NumericKeypad = new(["789", "456", "123", " 0A"]);
    private static readonly Map RemoteControlKeypad = new([" ^A", "<v>"]);
    private readonly List<Dictionary<string, long>> dp;
    private readonly Dictionary<Map, Dictionary<(char, char), string[]>> keypadWays;


    public Day21()
    {
        this.keypadWays = new Dictionary<Map, Dictionary<(char, char), string[]>>
        {
            [RemoteControlKeypad] = this.BuildPossibleWaysBetweenKeys(RemoteControlKeypad),
            [NumericKeypad] = this.BuildPossibleWaysBetweenKeys(NumericKeypad)
        };
        var allPotentialWays = this.keypadWays[RemoteControlKeypad].Values
            .Concat(this.keypadWays[NumericKeypad].Values)
            .SelectMany(x => x).Distinct().ToList();
        this.dp = [allPotentialWays.ToDictionary(x => x, x => (long)x.Length)];
        for (var i = 1; i <= 25; i++)
        {
            this.dp.Add(
                allPotentialWays.ToDictionary(
                    x => x,
                    x => this.GetShortestWayLength(this.dp[i - 1], RemoteControlKeypad, x)
                )
            );
        }
    }

    private Dictionary<(char, char), string[]> BuildPossibleWaysBetweenKeys(Map keypad)
    {
        var result = new Dictionary<(char, char), string[]>();
        foreach (var from in EnumerateKeypadPositions(keypad))
        foreach (var to in EnumerateKeypadPositions(keypad))
        {
            var fromKey = keypad[from];
            var toKey = keypad[to];
            result[(fromKey, toKey)] = this.GetWays(keypad, fromKey, toKey);
        }

        return result;
    }

    private static IEnumerable<V2> EnumerateKeypadPositions(Map keypad) =>
        keypad.EnumeratePositions().Where(x => keypad[x] != ' ');

    private string[] GetWays(Map keypad, char keyFrom, char keyTo)
    {
        if (keyFrom == keyTo)
        {
            return ["A"];
        }

        var start = keypad.EnumeratePositions().Single(x => keypad[x] == keyFrom);
        var end = keypad.EnumeratePositions().Single(x => keypad[x] == keyTo);
        var result = new List<string>();
        foreach (
            var next in start
                .GetNeighbours4()
                .Where(x => keypad.InBounds(x) && keypad[x] != ' ')
                .Where(x => (end - x).ManhattanLength() < (end - start).ManhattanLength())
        )
        {
            var ways = this.GetWays(keypad, keypad[next], keypad[end]);
            result.AddRange(Directions[next - start].SelectMany(x => ways.Select(y => $"{x}{y}")));
        }

        return result.ToArray();
    }

    private long GetShortestWayLength(Dictionary<string, long> dict, Map keypad, string code) =>
        $"A{code}".Zip(code,
            (a, b) => this.keypadWays[keypad][(a, b)].Select(x => dict[x]).Min()
        ).Sum();

    private long Complexity(int dpLevel, string code)
    {
        var shortestWayLength = this.GetShortestWayLength(this.dp[dpLevel], NumericKeypad, code);
        return shortestWayLength * int.Parse(new string(code.Where(char.IsDigit).ToArray()));
    }

    public override void Solve()
    {
        var codes = ReadBlockLines().ToArray();
        Console.WriteLine(codes.Select(code => this.Complexity(2, code)).Sum());
        Console.WriteLine(codes.Select(code => this.Complexity(25, code)).Sum());
    }
}
