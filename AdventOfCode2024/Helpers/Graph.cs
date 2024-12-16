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

    public static IEnumerable<PathItem<TVertex>> Dijikstra<TVertex>(IEnumerable<TVertex> startVertices,
        Func<TVertex, IEnumerable<(TVertex Vertex, long Distance)>> next)
        where TVertex : notnull
    {
        var queue = new PriorityQueue<TVertex, long>();
        var distance = new Dictionary<TVertex, long>();
        var path = new Dictionary<TVertex, PathItem<TVertex>>();
        foreach (var start in startVertices)
        {
            queue.Enqueue(start, 0);
            distance[start] = 0;
            path[start] = new PathItem<TVertex>(start, 0, null);
        }

        while (queue.TryDequeue(out var v, out var vDistance))
        {
            if (distance[v] != vDistance)
            {
                continue;
            }

            yield return path[v];
            foreach (var u in next(v))
            {
                if (distance.GetValueOrDefault(u.Vertex, long.MaxValue) > distance[v] + u.Distance)
                {
                    distance[u.Vertex] = distance[v] + u.Distance;
                    path[u.Vertex] = new PathItem<TVertex>(u.Vertex, distance[u.Vertex], path[v]);
                    queue.Enqueue(u.Vertex, distance[u.Vertex]);
                }
            }
        }
    }
}
