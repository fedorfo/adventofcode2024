namespace AdventOfCode2024.Base;

using System.Globalization;
using Helpers;

public abstract class PuzzleBase : IPuzzle
{
    public int Day => int.Parse(this.GetType().Name.Replace("Day", ""), CultureInfo.InvariantCulture);

    public virtual string InputFileName => $"{this.Day:00}.txt";
    public abstract void Solve();

    protected static IEnumerable<string> ReadBlockLines()
    {
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                yield break;
            }

            yield return line;
        }
    }

    protected static string ReadBlockText() => string.Join("\n", ReadBlockLines());

    protected static char[][] ReadCharMap() => ReadBlockLines().Select(x => x.ToArray()).ToArray();
    protected static Map ReadMap() => new(ReadCharMap());
}
