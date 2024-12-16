namespace AdventOfCode2024.Helpers;

using System.Numerics;

public readonly struct Fraction : IComparable
{
    public static Fraction Parse(string s)
    {
        var parts = s.Split('/');
        if (parts.Length == 1)
        {
            return new Fraction(BigInteger.Parse(parts[0]), BigInteger.One);
        }

        if (parts.Length != 2)
        {
            throw new FormatException(s);
        }

        return new Fraction(BigInteger.Parse(parts[0]), BigInteger.Parse(parts[1]));
    }

    public static readonly Fraction Zero = new(BigInteger.Zero, BigInteger.One);

    private readonly BigInteger numerator;
    private readonly BigInteger denominator;
    private readonly bool reduced;

    public Fraction(BigInteger numerator, BigInteger denominator)
        : this(numerator, denominator, false)
    {
    }

    public Fraction(BigInteger value)
        : this(value, BigInteger.One, true)
    {
    }

    private Fraction(BigInteger numerator, BigInteger denominator, bool reduced)
    {
        if (denominator == BigInteger.Zero)
        {
            throw new ArgumentException("Invalid value", nameof(denominator));
        }

        this.numerator = numerator;
        this.denominator = denominator;
        this.reduced = reduced;
    }

    public static BigInteger Lcm(BigInteger a, BigInteger b) => a * b / BigInteger.GreatestCommonDivisor(a, b);

    public Fraction Reduce()
    {
        if (this.reduced)
        {
            return this;
        }

        if (this.numerator == BigInteger.Zero)
        {
            return new Fraction(BigInteger.Zero, BigInteger.One, true);
        }

        var gcd = BigInteger.GreatestCommonDivisor(this.numerator, this.denominator);
        var n = this.numerator / gcd;
        var d = this.denominator / gcd;
        return d < 0 ? new Fraction(-n, -d, true) : new Fraction(n, d, true);
    }

    public readonly Fraction Abs() => this.IsNegative ? -this : this;

    public readonly int ToInt() => (int)BigInteger.Divide(this.numerator, this.denominator);

    public long ToLong() => (long)BigInteger.Divide(this.numerator, this.denominator);

    public bool IsInt()
    {
        this.Reduce();
        return this.denominator == BigInteger.One;
    }

    public readonly bool IsPositive => this.Sign > 0;
    public readonly bool IsNegative => this.Sign < 0;
    public readonly bool IsZero => this.numerator.IsZero;
    public readonly int Sign => this.numerator.Sign * this.denominator.Sign;

    public override string ToString()
    {
        if (this.numerator.IsZero)
        {
            return "0";
        }

        if (this.denominator.IsOne)
        {
            return this.numerator.ToString();
        }

        return $"{this.numerator}/{this.denominator}";
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is Fraction))
        {
            return false;
        }

        var r1 = this.Reduce();
        var r2 = ((Fraction)obj).Reduce();
        return r1.numerator == r2.numerator && r1.denominator == r2.denominator;
    }

    public override int GetHashCode()
    {
        var r = this.Reduce();
        unchecked
        {
            return (r.numerator.GetHashCode() * 397) ^ r.denominator.GetHashCode();
        }
    }

    public static Fraction operator +(Fraction r1, Fraction r2)
    {
        var nominator = (r1.numerator * r2.denominator) + (r2.numerator * r1.denominator);
        return new Fraction(
            nominator,
            nominator == BigInteger.Zero ? BigInteger.One : r1.denominator * r2.denominator
        ).Reduce();
    }

    public static Fraction operator -(Fraction r1, Fraction r2)
    {
        var nominator = (r1.numerator * r2.denominator) - (r2.numerator * r1.denominator);
        return new Fraction(
            nominator,
            nominator == BigInteger.Zero ? BigInteger.One : r1.denominator * r2.denominator
        ).Reduce();
    }

    public static Fraction operator *(Fraction a, Fraction b) =>
        new Fraction(a.numerator * b.numerator, a.denominator * b.denominator).Reduce();

    public static Fraction operator /(Fraction a, Fraction b) =>
        new Fraction(a.numerator * b.denominator, a.denominator * b.numerator).Reduce();


    public static Fraction operator +(Fraction r1, int n2) => r1 + new Fraction(n2, BigInteger.One, true);

    public static Fraction operator -(Fraction r) => new(-r.numerator, r.denominator, r.reduced);
    public static implicit operator Fraction(int r) => new(r, BigInteger.One, true);

    public static implicit operator Fraction(long r) => new(r, BigInteger.One, true);

    public static implicit operator Fraction(string s) => Parse(s);

    public static Fraction operator +(int n2, Fraction r1) => r1 + n2;

    public static implicit operator double(Fraction r1)
    {
        r1 = r1.Reduce();
        return (double)r1.numerator / (double)r1.denominator;
    }

    public static implicit operator float(Fraction r1)
    {
        r1 = r1.Reduce();
        return (float)((double)r1.numerator / (double)r1.denominator);
    }

    public int CompareTo(object? obj)
    {
        if (!(obj is Fraction))
        {
            throw new Exception();
        }

        var r = (Fraction)obj;
        return (this.numerator * r.denominator).CompareTo(this.denominator * r.numerator);
    }

    public static bool operator ==(Fraction r1, Fraction r2) => r1.Equals(r2);

    public static bool operator !=(Fraction r1, Fraction r2) => !r1.Equals(r2);

    public static bool operator <=(Fraction r1, Fraction r2) => r1.CompareTo(r2) < 1;

    public static bool operator >=(Fraction r1, Fraction r2) => r2 <= r1;

    public static bool operator <(Fraction r1, Fraction r2) => r1.CompareTo(r2) < 0;

    public static bool operator >(Fraction r1, Fraction r2) => r2 < r1;

    public static Fraction Max(Fraction a, Fraction b) => a > b ? a : b;

    public static Fraction Max(params Fraction[] values) => values.Max();
}
