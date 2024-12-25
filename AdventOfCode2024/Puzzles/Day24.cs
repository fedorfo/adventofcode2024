namespace AdventOfCode2024.Puzzles;

using System.Diagnostics;
using Helpers;
using MoreLinq;

public class Day24 : PuzzleBase
{
    private static bool AreGatesCorrect(List<Gate> gates)
    {
        for (var i = 0; i <= 44; i++)
        {
            if (CalculateZ((long)1 << i, (long)1 << i, gates) != (long)1 << (i + 1))
            {
                return false;
            }
        }

        for (var i = 0; i < 100; i++)
        {
            var x = Random.Shared.NextInt64((long)1 << 45);
            var y = Random.Shared.NextInt64((long)1 << 45);
            if (CalculateZ(x, y, gates) != x + y)
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryFillWires(
        Dictionary<string, bool> values,
        List<Gate> gates
    )
    {
        var gateByOut = gates.ToDictionary(x => x.Out);
        var edges = gates
            .Select(x => (x.In1, x.Out))
            .Concat(gates.Select(x => (x.In2, x.Out)))
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Item2).ToArray());
        var queue = new Queue<string>();
        values.Keys.ForEach(x => queue.Enqueue(x));
        var visited = new Dictionary<string, int>();
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            if (!values.ContainsKey(v))
            {
                var gate = gateByOut[v];
                values[gate.Out] = gate.Operator switch
                {
                    "AND" => values[gate.In1] & values[gate.In2],
                    "OR" => values[gate.In1] | values[gate.In2],
                    "XOR" => values[gate.In1] ^ values[gate.In2]
                };
            }

            foreach (var u in edges.GetValueOrDefault(v, []))
            {
                visited.TryAdd(u, 0);
                visited[u]++;
                if (visited[u] == 2)
                {
                    queue.Enqueue(u);
                }
            }
        }

        for (var i = 0; i <= 45; i++)
        {
            if (!values.ContainsKey($"z{i:00}"))
            {
                return false;
            }
        }

        return true;
    }

    private static long? CalculateZ(long x, long y, List<Gate> gates)
    {
        var values = new Dictionary<string, bool>();
        for (var i = 0; i <= 44; i++)
        {
            values[$"x{i:00}"] = (x & ((long)1 << i)) != 0;
            values[$"y{i:00}"] = (y & ((long)1 << i)) != 0;
        }

        if (!TryFillWires(values, gates))
        {
            return null;
        }

        return GetNumber("z", values);
    }

    private static long GetNumber(string prefix, Dictionary<string, bool> wires) =>
        wires.Keys.Where(x => x.StartsWith(prefix))
            .OrderByDescending(x => x)
            .Select(x => wires[x] ? 1 : 0)
            .Aggregate((long)0, (current, x) => (current << 1) | (uint)x);

    public override void Solve()
    {
        var wireValues = ReadBlockLines()
            .Select(x => x.ExtractTokens(' ', ':').ToArray())
            .ToDictionary(x => x[0], x => x[1] == "1");
        var gates = ReadBlockLines()
            .Select(x => x.ExtractTokens(' ', '-', '>').ToArray())
            .Select(x => new Gate(x[0], x[2], x[1], x[3]))
            .ToList();
        Console.WriteLine(CalculateZ(GetNumber("x", wireValues), GetNumber("y", wireValues), gates));
        var candidates = new List<(Gate, Gate)>();
        var sw = Stopwatch.StartNew();
        for (var i = 0; i <= 44; i++)
        {
            if (CalculateZ((long)1 << i, (long)1 << i, gates) == (long)1 << (i + 1))
            {
                continue;
            }

            for (var j = 0; j < gates.Count; j++)
            {
                for (var k = j + 1; k < gates.Count; k++)
                {
                    var gate1 = gates[j];
                    var gate2 = gates[k];
                    (gate1.Out, gate2.Out) = (gate2.Out, gate1.Out);
                    if (CalculateZ((long)1 << i, (long)1 << i, gates) == (long)1 << (i + 1))
                    {
                        candidates.Add((gate1, gate2));
                    }

                    (gate1.Out, gate2.Out) = (gate2.Out, gate1.Out);
                }
            }
        }
        Console.WriteLine(sw.Elapsed);

        var sw2 = Stopwatch.StartNew();
        for (var i = 0; i < candidates.Count; i++)
        for (var j = i; j < candidates.Count; j++)
        for (var k = j; k < candidates.Count; k++)
        for (var l = k; l < candidates.Count; l++)
        {
            if (
                new List<string>
                {
                    candidates[i].Item1.Out,
                    candidates[i].Item2.Out,
                    candidates[j].Item1.Out,
                    candidates[j].Item2.Out,
                    candidates[k].Item1.Out,
                    candidates[k].Item2.Out,
                    candidates[l].Item1.Out,
                    candidates[l].Item2.Out
                }.Distinct().Count() != 8
            )
            {
                continue;
            }

            (candidates[i].Item1.Out, candidates[i].Item2.Out) = (candidates[i].Item2.Out, candidates[i].Item1.Out);
            (candidates[j].Item1.Out, candidates[j].Item2.Out) = (candidates[j].Item2.Out, candidates[j].Item1.Out);
            (candidates[k].Item1.Out, candidates[k].Item2.Out) = (candidates[k].Item2.Out, candidates[k].Item1.Out);
            (candidates[l].Item1.Out, candidates[l].Item2.Out) = (candidates[l].Item2.Out, candidates[l].Item1.Out);

            if (AreGatesCorrect(gates))
            {
                Console.WriteLine(
                    string.Join(",",
                        new[]
                        {
                            candidates[i].Item1.Out, candidates[i].Item2.Out, candidates[j].Item1.Out,
                            candidates[j].Item2.Out, candidates[k].Item1.Out, candidates[k].Item2.Out,
                            candidates[l].Item1.Out, candidates[l].Item2.Out
                        }.OrderBy(x => x)));
                Console.WriteLine(sw2.Elapsed);
                return;
            }

            (candidates[i].Item1.Out, candidates[i].Item2.Out) = (candidates[i].Item2.Out, candidates[i].Item1.Out);
            (candidates[j].Item1.Out, candidates[j].Item2.Out) = (candidates[j].Item2.Out, candidates[j].Item1.Out);
            (candidates[k].Item1.Out, candidates[k].Item2.Out) = (candidates[k].Item2.Out, candidates[k].Item1.Out);
            (candidates[l].Item1.Out, candidates[l].Item2.Out) = (candidates[l].Item2.Out, candidates[l].Item1.Out);
        }
    }

    private class Gate(string in1, string in2, string @operator, string @out)
    {
        public string In1 { get; } = in1;
        public string In2 { get; } = in2;
        public string Operator { get; } = @operator;
        public string Out { get; set; } = @out;
    }
}
