namespace AdventOfCode2024.Helpers;

public interface IGraph<TVertex>
{
    IEnumerable<TVertex> GetVertices();
    IEnumerable<(TVertex Vertex, long Distance)> GetEdges(TVertex v);
}

public static class GraphAlgo
{
    public static (Dictionary<TVertex, long> Distance, Dictionary<TVertex, (TVertex Parent, long Distance)> Path)
        Dijikstra<TVertex>(IGraph<TVertex> graph, params TVertex[] startVertices)
        where TVertex : notnull
    {
        var queue = new PriorityQueue<TVertex, long>();
        var distance = new Dictionary<TVertex, long>();
        var path = new Dictionary<TVertex, (TVertex Parent, long Distance)>();
        foreach (var start in startVertices)
        {
            queue.Enqueue(start, 0);
            distance[start] = 0;
            path[start] = (start, 0);
        }

        while (queue.TryDequeue(out var v, out var vDistance))
        {
            if (distance[v] != vDistance)
            {
                continue;
            }

            foreach (var u in graph.GetEdges(v))
            {
                if (distance.GetValueOrDefault(u.Vertex, long.MaxValue) > distance[v] + u.Distance)
                {
                    distance[u.Vertex] = distance[v] + u.Distance;
                    path[u.Vertex] = (v, u.Distance);
                    queue.Enqueue(u.Vertex, distance[u.Vertex]);
                }
            }
        }

        return (distance, path);
    }
}
