using IbadB.Graphs.Algorithms;

namespace IbadB.Graphs.Tests.Algorithms;

public class DepthFirstSearchTests
{
    private static Graph<string, int> BuildGraph(
        out GraphNode<string, int> a,
        out GraphNode<string, int> b,
        out GraphNode<string, int> c,
        out GraphNode<string, int> d)
    {
        var graph = new Graph<string, int>();

        a = new GraphNode<string, int>("A");
        b = new GraphNode<string, int>("B");
        c = new GraphNode<string, int>("C");
        d = new GraphNode<string, int>("D");

        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddNode(c);
        graph.AddNode(d);

        return graph;
    }

    [Fact]
    public void DFS_ReturnsPath_InLinearGraph()
    {
        // A -> B -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.DFS(a, d);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void DFS_ReturnsAnyValidPath_WhenMultipleRoutesExist()
    {
        // A -> B -> D
        // A -> C -> D
        // DFS not guarantees the shortest path — just a valid one
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.DFS(a, d);

        Assert.Equal(a.Id, result.Vertexes.First().Id);
        Assert.Equal(d.Id, result.Vertexes.Last().Id);
    }

    [Fact]
    public void DFS_GoesDeep_BeforeExploringAlternatives()
    {
        // A -> B -> C -> D
        // A -> D (direct path)
        // DFS will go deep through B -> C -> D, not immediately A -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);
        graph.AddEdge(a, d, 1);

        var result = graph.DFS(a, d);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    // ===== Edge Cases =====

    [Fact]
    public void DFS_ReturnsSingleNode_WhenStartEqualsEnd()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        graph.AddNode(a);

        var result = graph.DFS(a, a);

        Assert.Single(result.Vertexes);
        Assert.Equal(a.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void DFS_ReturnsEmpty_WhenNoPathExists()
    {
        // A and B are not connected — the stack will empty, _GetPath will return an empty list
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);

        var result = graph.DFS(a, b);

        Assert.Empty(result.Vertexes);
    }

    [Fact]
    public void DFS_HandlesGraph_WithCycles()
    {
        // A -> B -> C -> A (cycle)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, a, 1);

        var result = graph.DFS(a, c);

        Assert.Equal(new[] { a.Id, b.Id, c.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void DFS_EachNodeVisitedOnce_WhenMultiplePathsExist()
    {
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.DFS(a, d);

        Assert.Equal(1, result.Vertexes.Count(n => n.Id == d.Id));
    }
}