namespace AdventOfCode2024.Puzzles;

using Helpers;

public class Day23 : PuzzleBase
{
    public override void Solve()
    {
        var links = ReadBlockLines().Select(x => x.Split("-").Where(x => !string.IsNullOrEmpty(x)).ToArray()).ToArray();
        var computers = links.Select(x => x[0]).Concat(links.Select(x => x[1])).Distinct().OrderBy(x => x).ToArray();
        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var v in computers)
        {
            graph[v] = new HashSet<string>();
        }

        foreach (var link in links)
        {
            graph[link[0]].Add(link[1]);
            graph[link[1]].Add(link[0]);
        }

        var ans1 = 0;
        for (var i = 0; i < computers.Length; i++)
        for (var j = i + 1; j < computers.Length; j++)
        for (var k = j + 1; k < computers.Length; k++)
        {
            if (computers[i][0] != 't' && computers[j][0] != 't' && computers[k][0] != 't')
            {
                continue;
            }

            if (graph[computers[i]].Contains(computers[j]) &&
                graph[computers[j]].Contains(computers[k]) &&
                graph[computers[i]].Contains(computers[k]))
            {
                ans1++;
            }
        }

        Console.WriteLine(ans1);

        var cliques = GraphAlgo.BronKerbosch(x => graph[x], [..computers]);
        var largestClique = cliques.OrderByDescending(c => c.Count).First();
        Console.WriteLine(string.Join(',', largestClique.OrderBy(x => x)));
    }
}
