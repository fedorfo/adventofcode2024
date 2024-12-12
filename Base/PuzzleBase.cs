namespace AdventOfCode2024.Base;

using System.Globalization;
using Helpers;

public abstract class PuzzleBase : IPuzzle
{
    public int Day => int.Parse(this.GetType().Name.Replace("Day", ""), CultureInfo.InvariantCulture);

    public virtual string InputFileName => $"{this.Day:00}.txt";
    public abstract void Solve();

    protected static List<string> ReadLines()
    {
        var result = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (line is null)
            {
                return result;
            }

            result.Add(line);
        }
    }

    protected static string ReadAll() => string.Join("\n", ReadLines());

    protected static char[][] ReadCharMap() => ReadLines().Select(x => x.ToArray()).ToArray();
    protected static Map ReadMap() => new(ReadCharMap());
}
