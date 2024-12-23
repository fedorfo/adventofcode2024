namespace AdventOfCode2024.Puzzles;

public class Day22 : PuzzleBase
{
    private static long Modulo(long a, long b) => ((a % b) + b) % b;

    private static long Next(long secretNumber)
    {
        secretNumber = Modulo(secretNumber ^ (secretNumber << 6), 16777216);
        secretNumber = Modulo(secretNumber ^ (secretNumber >> 5), 16777216);
        return Modulo(secretNumber ^ (secretNumber << 11), 16777216);
    }

    private static long Next(long secretNumber, int iterations)
    {
        while (iterations > 0)
        {
            secretNumber = Next(secretNumber);
            iterations--;
        }

        return secretNumber;
    }


    private Dictionary<(int, int, int, int), int> GetChanges(long secretNumber, int iterations)
    {
        var numbers = new List<long> { secretNumber };
        for (var i = 0; i < iterations; i++)
        {
            secretNumber = Next(secretNumber);
            numbers.Add(secretNumber);
        }

        var bananas = numbers.Select(x => (int)(x % 10)).ToArray();
        var changes = bananas.Zip(bananas.Skip(1), (a, b) => b - a).ToArray();
        var result = new Dictionary<(int, int, int, int), int>();
        for (var i = 3; i < changes.Length; i++)
        {
            var key = (changes[i - 3], changes[i - 2], changes[i - 1], changes[i]);
            result.TryAdd(key, bananas[i + 1]);
        }

        return result;
    }

    public override void Solve()
    {
        var secretNumbers = ReadBlockLines().Select(int.Parse).ToArray();
        Console.WriteLine(secretNumbers.Select(x => Next(x, 2000)).Sum());

        var changes = secretNumbers.Select(x => this.GetChanges(x, 2000)).ToArray();
        var answer = new Dictionary<(int, int, int, int), int>();
        foreach (var change in changes)
        {
            foreach (var key in change.Keys)
            {
                answer.TryAdd(key, 0);
                answer[key] += change[key];
            }
        }
        Console.WriteLine(answer.Values.Max());
    }
}
