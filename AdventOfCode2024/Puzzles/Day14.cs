namespace AdventOfCode2024.Puzzles;

using System.Text.RegularExpressions;
using Base;
using Helpers;

public class Day14 : PuzzleBase
{
    private const int Width = 101; //11;
    private const int Height = 103; // 7;


    private int Normalize(int x, int maxValue)
    {
        x %= maxValue;
        if (x < 0)
        {
            x += maxValue;
        }

        return x;
    }

    private List<V2> GetPositions(List<(V2 P, V2 V)> robots, int time) =>
        robots
            .Select(x => x.P + (x.V * time))
            .Select(x => new V2(this.Normalize(x.X, Width), this.Normalize(x.Y, Height)))
            .ToList();

    private bool LooksLikeChristmasTree(List<V2> positions)
    {
        var positionsSet = positions.ToHashSet();
        var visited = new HashSet<V2>();
        var components = new List<List<V2>>();

        foreach (var v in positions)
        {
            if (!visited.Contains(v))
            {
                var area = GraphAlgo.Bfs(
                    [v],
                    x => x.GetNeighbours4().Where(y => positionsSet.Contains(y))
                ).Select(x => x.Vertex).ToList();
                visited.UnionWith(area);
                components.Add(area);
            }
        }

        components = components.OrderByDescending(x => x.Count).ToList();
        return components[0].Count > positions.Count * 4 / 10;
    }

    public override void Solve()
    {
        var robots = ReadBlockLines()
            .Select(
                line => Regex
                    .Replace(line, "[a-zA-Z=,]", " ")
                    .ExtractTokens()
                    .Select(int.Parse)
                    .ToArray())
            .Select(x => (P: new V2(x[0], x[1]), V: new V2(x[2], x[3]))).ToList();
        var positions100 = this.GetPositions(robots, 100);
        var ranges = new[]
        {
            (Start: new V2(0, 0), End: new V2((Width / 2) - 1, (Height / 2) - 1)),
            (Start: new V2(0, (Height / 2) + 1), End: new V2((Width / 2) - 1, Height - 1)),
            (Start: new V2((Width / 2) + 1, 0), End: new V2(Width - 1, (Height / 2) - 1)),
            (Start: new V2((Width / 2) + 1, (Height / 2) + 1), End: new V2(Width - 1, Height - 1))
        };

        var result = ranges
            .Select(
                range => positions100.Count(
                    x => range.Start <= x && x <= range.End))
            .ToArray();
        Console.WriteLine(result.Aggregate(1, (x, res) => x * res));


        for (var step = 0; step < 100000; step++)
        {
            var positions = this.GetPositions(robots, step);
            if (this.LooksLikeChristmasTree(positions))
            {
                Console.WriteLine(step);
                break;
            }
        }
    }
}
