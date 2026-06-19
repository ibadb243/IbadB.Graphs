using IbadB.Graphs.Algorithms;

namespace IbadB.Graphs.Tests.Algorithms;

public class AStarTests
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

    // Heuristic — approximate distance to D
    private static readonly Dictionary<string, int> _heuristic = new()
    {
        ["A"] = 3,
        ["B"] = 2,
        ["C"] = 1,
        ["D"] = 0
    };

    private static int H(GraphNode<string, int> node) =>
        _heuristic.TryGetValue(node.Model, out var h) ? h : int.MaxValue;

    [Fact]
    public void AStar_ReturnsPath_InLinearGraph()
    {
        // A -> B -> C -> D
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.AStar(a, d, H);

        Assert.Equal(new[] { a.Id, b.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void AStar_FindsShortestPath_WhenMultipleRoutesExist()
    {
        // A -> B -> D (g=1+5=6)
        // A -> C -> D (g=1+1=2)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 5);
        graph.AddEdge(c, d, 1);

        var result = graph.AStar(a, d, H);

        Assert.Equal(new[] { a.Id, c.Id, d.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void AStar_PrefersLowerCost_OverFewerHops()
    {
        // A -> B (g=10, 1 step)
        // A -> C -> B (g=1+1=2, 2 steps)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 10);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(c, b, 1);

        var result = graph.AStar(a, b, H);

        Assert.Equal(new[] { a.Id, c.Id, b.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void AStar_WithZeroHeuristic_BehavesLikeDijkstra()
    {
        // h=0 for all nodes => A* = Dijkstra
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 5);
        graph.AddEdge(c, d, 1);

        var resultAStar = graph.AStar(a, d, _ => 0);
        var resultDijkstra = graph.Dijkstra(a, d);

        Assert.Equal(
            resultDijkstra.Vertexes.Select(n => n.Id),
            resultAStar.Vertexes.Select(n => n.Id));
    }

    // ===== Edge Cases =====

    [Fact]
    public void AStar_ReturnsSingleNode_WhenStartEqualsEnd()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        graph.AddNode(a);

        var result = graph.AStar(a, a, H);

        Assert.Single(result.Vertexes);
        Assert.Equal(a.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void AStar_ReturnsOnlyEndNode_WhenNoPathExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);

        var result = graph.AStar(a, b, H);

        Assert.Single(result.Vertexes);
        Assert.Equal(b.Id, result.Vertexes[0].Id);
    }

    [Fact]
    public void AStar_HandlesGraph_WithCycles()
    {
        // A -> B -> C -> A (cyrcle)
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(b, c, 1);
        graph.AddEdge(c, a, 1);

        var result = graph.AStar(a, c, H);

        Assert.Equal(new[] { a.Id, b.Id, c.Id }, result.Vertexes.Select(n => n.Id));
    }

    [Fact]
    public void AStar_EachNodeProcessedOnce_WhenMultiplePathsExist()
    {
        var graph = BuildGraph(out var a, out var b, out var c, out var d);
        graph.AddEdge(a, b, 1);
        graph.AddEdge(a, c, 1);
        graph.AddEdge(b, d, 1);
        graph.AddEdge(c, d, 1);

        var result = graph.AStar(a, d, H);

        Assert.Equal(1, result.Vertexes.Count(n => n.Id == d.Id));
    }
}
