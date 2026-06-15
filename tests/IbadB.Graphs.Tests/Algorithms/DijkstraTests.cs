using IbadB.Graphs.Algorithms;

namespace IbadB.Graphs.Tests.Algorithms;

public class DijkstraTests
{
    private static Graph<string, int> BuildGraph(
        out GraphNode<string, int> a,
        out GraphNode<string, int> b,
        out GraphNode<string, int> c,
        out GraphNode<string, int> d)
    {
        var graph = new Graph<string, int>();

        a = new GraphNode<string, int>("A", "A");
        b = new GraphNode<string, int>("B", "B");
        c = new GraphNode<string, int>("C", "C");
        d = new GraphNode<string, int>("D", "D");

        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddNode(c);
        graph.AddNode(d);

        return graph;
    }

    [Fact]
    public void Dijkstra_ReturnsPath_InLinearGraph()
    {
        // A -> B -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.Dijkstra(a, d);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void Dijkstra_PrefersLowerWeight_OverFewerHops()
    {
        // A -> B (weight 10, 1 step)
        // A -> C -> B (weight 1+1=2, 2 steps)
        // Dijkstra should choose A -> C -> B
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 10);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(c, b, 1);

        var result = graph.Dijkstra(a, b);

        Assert.Equal(new[] { a.Id, c.Id, b.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void Dijkstra_FindsShortestPath_WhenMultipleRoutesExist()
    {
        // A -> B -> D (weight 1+5=6)
        // A -> C -> D (weight 1+1=2)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 5);
        graph.AddEdge(c, d, 1);

        var result = graph.Dijkstra(a, d);

        Assert.Equal(new[] { a.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    // ===== Edge Cases =====

    [Fact]
    public void Dijkstra_ReturnsSingleNode_WhenStartEqualsEnd()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        graph.AddNode(a);

        var result = graph.Dijkstra(a, a);

        Assert.Single(result.Vertexes);
        Assert.Equal(a.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void Dijkstra_ReturnsOnlyEndNode_WhenNoPathExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        graph.AddNode(a);
        graph.AddNode(b);

        var result = graph.Dijkstra(a, b);

        Assert.Single(result.Vertexes);
        Assert.Equal(b.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void Dijkstra_HandlesGraph_WithCycles()
    {
        // A -> B -> C -> A (cycle)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, a, 1);

        var result = graph.Dijkstra(a, c);

        Assert.Equal(new[] { a.Id, b.Id, c.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void Dijkstra_EachNodeProcessedOnce_WhenMultiplePathsExist()
    {
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.Dijkstra(a, d);

        Assert.Equal(1, result.Vertexes.Count(n => n.Id == d.Id));
    }
}