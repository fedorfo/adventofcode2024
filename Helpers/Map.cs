namespace AdventOfCode2024.Helpers;

public record Map(char[][] map)
{
    public char this[V2 p] => this.map[p.X][p.Y];
    public bool InBounds(V2 p) => p >= V2.Zero && p < new V2(this.map.Length, this.map[0].Length);

    public IEnumerable<V2> EnumeratePositions() =>
        V2.EnumerateRange(V2.Zero, new V2(this.map.Length, this.map[0].Length));
}
