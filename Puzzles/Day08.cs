namespace AdventOfCode2024.Puzzles;

using Base;
using Helpers;

public class Day08 : PuzzleBase
{
    private HashSet<V2> GetAntiNodes(Dictionary<char, V2[]> nodesByF, V2 mapSize, int iterations = 1)
    {
        var antiNodes = new HashSet<V2>();
        foreach (var key in nodesByF.Keys)
        {
            var nodeGroup = nodesByF[key];
            for (var i = 0; i < nodeGroup.Length; i++)
            for (var j = i + 1; j < nodeGroup.Length; j++)
            {
                var n1 = nodeGroup[i];
                var n2 = nodeGroup[j];
                if (iterations != 1)
                {
                    antiNodes.Add(n1);
                    antiNodes.Add(n2);
                }

                for (var it = 1; it <= iterations; it++)
                {
                    var candidate = n1 + ((n1 - n2) * it);
                    if (!(candidate >= V2.Zero && candidate < mapSize))
                    {
                        break;
                    }

                    antiNodes.Add(candidate);
                }

                for (var it = 1; it <= iterations; it++)
                {
                    var candidate = n2 + ((n2 - n1) * it);
                    if (!(candidate >= V2.Zero && candidate < mapSize))
                    {
                        break;
                    }

                    antiNodes.Add(candidate);
                }
            }
        }

        return antiNodes;
    }

    public override void Solve()
    {
        var map = ReadMap();
        var mapSize = new V2(map.Length, map[0].Length);
        var nodes = new List<(char F, V2 P)>();
        for (var i = 0; i < map.Length; i++)
        for (var j = 0; j < map.Length; j++)
        {
            if (map[i][j] != '.')
            {
                nodes.Add((map[i][j], new V2(i, j)));
            }
        }

        var nodesByF = nodes.GroupBy(x => x.F).ToDictionary(x => x.Key, x => x.Select(y => y.P).ToArray());
        Console.WriteLine(this.GetAntiNodes(nodesByF, mapSize).Count);
        Console.WriteLine(this.GetAntiNodes(nodesByF, mapSize, int.Max(mapSize.X, mapSize.Y)).Count);
    }
}
