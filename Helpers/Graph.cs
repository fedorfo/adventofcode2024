namespace AdventOfCode2024.Helpers;

public static class GraphAlgo
{
    public static IEnumerable<PathItem<TVertex>> Bfs<TVertex>(IEnumerable<TVertex> start,
        Func<TVertex, IEnumerable<TVertex>> next)
        where TVertex : notnull
    {
        var visited = new Dictionary<TVertex, PathItem<TVertex>>();
        var queue = new Queue<TVertex>();
        foreach (var v in start)
        {
            queue.Enqueue(v);
            visited[v] = new PathItem<TVertex>(v, 0, null);
            yield return visited[v];
        }

        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            foreach (var u in next(v))
            {
                if (visited.TryAdd(u, new PathItem<TVertex>(u, visited[v].Distance + 1, visited[v])))
                {
                    yield return visited[u];
                    queue.Enqueue(u);
                }
            }
        }
    }
}
