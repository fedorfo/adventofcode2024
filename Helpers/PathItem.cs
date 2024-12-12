namespace AdventOfCode2024.Helpers;

public record PathItem<TState>(TState Vertex, long Distance, PathItem<TState>? Prev)
{
    public IEnumerable<TState> PathBack()
    {
        for (var c = this; c != null; c = c.Prev)
        {
            yield return c.Vertex;
        }
    }

    public IEnumerable<TState> Path() => this.PathBack().Reverse();
}
