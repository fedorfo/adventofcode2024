namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day08 : PuzzleBase
{
    private static HashSet<V2> GetAntiNodes(Dictionary<char, V2[]> nodesByF, V2 mapSize, int iterations = 1)
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
        var nodes = new List<(char F, V2 P)>();
        foreach (var v in map.EnumeratePositions())
        {
            if (map[v] != '.')
            {
                nodes.Add((map[v], v));
            }
        }

        var nodesByF = nodes.GroupBy(x => x.F).ToDictionary(x => x.Key, x => x.Select(y => y.P).ToArray());
        Console.WriteLine(GetAntiNodes(nodesByF, map.Size).Count);
        Console.WriteLine(GetAntiNodes(nodesByF, map.Size, int.Max(map.Size.X, map.Size.Y)).Count);
    }
}
