namespace AdventOfCode2024.Helpers;

public static class Helpers
{
    public static int Max(params int[] values) => values.Max();

    public static long LongPow(long x, int y)
    {
        long result = 1;
        for (var i = 1; i <= y; i++)
        {
            result *= x;
        }

        return result;
    }

    public static int Mod(int a, int b)
    {
        var result = a % b;
        if (result < 0)
        {
            result += b;
        }

        return result;
    }

    public static void Measure(Action acc)
    {
        var now = DateTime.UtcNow;
        acc();
        Console.WriteLine(DateTime.UtcNow - now);
    }

    public static T Measure<T>(Func<T> func)
    {
        var now = DateTime.UtcNow;
        var result = func();
        Console.WriteLine(DateTime.UtcNow - now);
        return result;
    }

    public static IEnumerable<string> ExtractTokens(this string line, params char[] delimiters)
    {
        if (delimiters.Length == 0)
        {
            delimiters = new[] { ' ' };
        }

        return line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static long Gcd(long a, long b) => b > 0 ? Gcd(b, a % b) : a;

    public static long Lcd(long a, long b) => a * b / Gcd(a, b);

    public static Fraction[] SolveLinearSystem(Fraction[,] A, Fraction[] b)
    {
        var x = new Fraction[b.Length];
        for (var i = 0; i < b.Length; i++)
        {
            for (var j = i + 1; j < b.Length; j++)
            {
                if (A[j, i] != Fraction.Zero)
                {
                    var factor = A[j, i] / A[i, i];
                    for (var k = i; k < b.Length; k++)
                    {
                        A[j, k] -= factor * A[i, k];
                    }

                    b[j] -= factor * b[i];
                }
            }
        }

        for (var i = b.Length - 1; i >= 0; i--)
        {
            var sum = Fraction.Zero;
            for (var j = i + 1; j < b.Length; j++)
            {
                sum += A[i, j] * x[j];
            }

            x[i] = (b[i] - sum) / A[i, i];
        }

        return x;
    }

    public static int BinarySearch<T>(T[] array, Func<int, bool> gte)
    {
        var l = 0;
        var r = array.Length - 1;
        while (l < r)
        {
            var m = (l + r) / 2;
            if (gte(m))
            {
                r = m;
            }
            else
            {
                l = m + 1;
            }
        }

        return l;
    }
}
