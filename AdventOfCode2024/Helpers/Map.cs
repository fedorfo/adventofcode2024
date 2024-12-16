namespace AdventOfCode2024.Helpers;

using MoreLinq.Extensions;

public record Map(char[][] map)
{
    public char this[V2 p]
    {
        get => this.map[p.X][p.Y];
        set => this.map[p.X][p.Y] = value;
    }

    public V2 Size => new(this.map.Length, this.map[0].Length);

    public char[][] GetMap() => this.map;
    public bool InBounds(V2 p) => p >= V2.Zero && p < this.Size;
    public IEnumerable<V2> EnumeratePositions() => V2.EnumerateRange(V2.Zero, this.Size);
    public void Print() => this.map.ForEach(row => Console.WriteLine(new string(row)));
    public Map Copy() => new(this.map.Select(x => x.ToArray()).ToArray());
}
