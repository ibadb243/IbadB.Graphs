using IbadB.Graphs.Algorithms;

namespace IbadB.Graphs.Tests.Algorithms;

public class BreadthFirstSearchTests
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
    public void BFS_ReturnsPath_InLinearGraph()
    {
        // A -> B -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.BFS(a, d);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void BFS_ReturnsShortestPath_WhenMultipleRoutesExist()
    {
        // A -> B -> D (2 steps)
        // A -> C -> D (2 steps)
        // BFS should find one of them with a length of 3 nodes
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.BFS(a, d);

        Assert.Equal(a.Id, result.Vertexes.First().Id);
        Assert.Equal(d.Id, result.Vertexes.Last().Id);
        Assert.Equal(3, result.Vertexes.Count);
    }

    [Fact]
    public void BFS_PrefersFewerHops_OverDirectHighWeightEdge()
    {
        // A -> B (1 step, weight 100)
        // A -> C -> B (2 steps, weight 1+1)
        // BFS does not consider weights — should choose the direct path A -> B
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 100);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(c, b, 1);

        var result = graph.BFS(a, b);

        Assert.Equal(new[] { a.Id, b.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void BFS_ReturnsSingleNode_WhenStartEqualsEnd()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        graph.AddNode(a);

        var result = graph.BFS(a, a);

        Assert.Single(result.Vertexes);
        Assert.Equal(a.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void BFS_ReturnsOnlyEndNode_WhenNoPathExists()
    {
        // A and B are not connected — path will be empty,
        // _GetPath will return only end (current behavior)
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        graph.AddNode(a);
        graph.AddNode(b);

        var result = graph.BFS(a, b);

        Assert.Single(result.Vertexes);
        Assert.Equal(b.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void BFS_HandlesGraph_WithCycles()
    {
        // A -> B -> C -> A (cycle)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, a, 1);

        var result = graph.BFS(a, c);

        Assert.Equal(new[] { a.Id, b.Id, c.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void BFS_EachNodeEnqueuedOnce_WhenMultiplePathsExist()
    {
        // A -> B -> D
        // A -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.BFS(a, d);

        Assert.Equal(1, result.Vertexes.Count(n => n.Id == d.Id));
    }
}