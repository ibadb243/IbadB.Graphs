using IbadB.Graphs.Algorithms;

namespace IbadB.Graphs.Tests.Algorithms;

public class GreedyBestFirstSearchTests
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

    // Heuristic — distance to D by alphabet
    // A=3, B=2, C=1, D=0
    private static readonly Dictionary<string, int> _heuristic = new()
    {
        ["A"] = 3,
        ["B"] = 2,
        ["C"] = 1,
        ["D"] = 0
    };

    private static int H(GraphNode<string, int> node) =>
        _heuristic.TryGetValue(node.Name, out var h) ? h : int.MaxValue;

    [Fact]
    public void GBFS_ReturnsPath_InLinearGraph()
    {
        // A -> B -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.GBFS(a, d, H);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void GBFS_FollowsHeuristic_WhenMultipleNeighborsExist()
    {
        // A -> B (h=2), A -> C (h=1)
        // GBFS will choose C first, as h(C) < h(B)
        // C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.GBFS(a, d, H);

        Assert.Equal(new[] { a.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void GBFS_MayNotFindShortestPath_WhenHeuristicMisleads()
    {
        // GBFS does not guarantee the shortest path —
        // it follows the heuristic, even if the path is longer
        // A -> B -> D (2 steps, h(B)=2)
        // A -> C -> D (2 steps, h(C)=1) — GBFS will choose this
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.GBFS(a, d, H);

        Assert.Equal(a.Id, result.Vertexes.First().Id);
        Assert.Equal(d.Id, result.Vertexes.Last().Id);
    }

    // ===== Edge Cases =====

    [Fact]
    public void GBFS_ReturnsSingleNode_WhenStartEqualsEnd()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        graph.AddNode(a);

        var result = graph.GBFS(a, a, H);

        Assert.Single(result.Vertexes);
        Assert.Equal(a.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void GBFS_ReturnsEmpty_WhenNoPathExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        graph.AddNode(a);
        graph.AddNode(b);

        var result = graph.GBFS(a, b, H);

        Assert.Empty(result.Vertexes);
    }

    [Fact]
    public void GBFS_HandlesGraph_WithCycles()
    {
        // A -> B -> C -> A (cyrcle)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, a, 1);

        var result = graph.GBFS(a, c, H);

        Assert.Equal(new[] { a.Id, b.Id, c.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void GBFS_EachNodeVisitedOnce_WhenMultiplePathsExist()
    {
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.GBFS(a, d, H);

        Assert.Equal(1, result.Vertexes.Count(n => n.Id == d.Id));
    }
}