using static System.Math;

namespace AdventOfCode2024.Helpers;

public record V2(int X, int Y)
{
    public static readonly V2 Zero = new(0, 0);

    public static IEnumerable<V2> EnumerateRange(V2 start, V2 end)
    {
        for (var i = start.X; i != end.X; i += start.X < end.X ? 1 : -1)
        for (var j = start.Y; j != end.Y; j += start.Y < end.Y ? 1 : -1)
        {
            yield return new V2(i, j);
        }
    }

    public static V2 operator +(V2 l, V2 r) => new(l.X + r.X, l.Y + r.Y);

    public static V2 operator -(V2 l, V2 r) => new(l.X - r.X, l.Y - r.Y);

    public static V2 operator /(V2 p, int l) => new(p.X / l, p.Y / l);
    public static V2 operator *(V2 p, int l) => new(p.X * l, p.Y * l);

    public static bool operator <(V2 l, V2 r) => l.X < r.X && l.Y < r.Y;

    public static bool operator <=(V2 l, V2 r) => l.X <= r.X && l.Y <= r.Y;

    public static bool operator >=(V2 l, V2 r) => r <= l;

    public static bool operator >(V2 l, V2 r) => r < l;

    public int ManhattanLength() => Abs(this.X) + Abs(this.Y);

    public int ChebyshevLength() => Max(Abs(this.X), Abs(this.Y));

    public IEnumerable<V2> GetNeighbours8()
    {
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        {
            var candidate = new V2(x, y);
            if (candidate.ChebyshevLength() == 1)
            {
                yield return this + candidate;
            }
        }
    }

    public IEnumerable<V2> GetNeighbours4()
    {
        for (var x = -1; x <= 1; x++)
        for (var y = -1; y <= 1; y++)
        {
            var candidate = new V2(x, y);
            if (candidate.ManhattanLength() == 1)
            {
                yield return this + candidate;
            }
        }
    }

    public V2 CCW() => new(-this.Y, this.X);
    public V2 CW() => new(this.Y, -this.X);

    public override string ToString() => $"({this.X},{this.Y})";
}
