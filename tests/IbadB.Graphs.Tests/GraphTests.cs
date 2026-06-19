namespace IbadB.Graphs.Tests;

public class GraphTests
{
    [Fact]
    public void AddNode_AddsNodeToGraph()
    {
        var graph = new Graph<string, int>();
        var node = new GraphNode<string, int>("A");

        graph.AddNode(node);

        Assert.Contains(node.Id, graph.Nodes);
    }

    [Fact]
    public void AddNode_Duplicate_IsIgnored()
    {
        var graph = new Graph<string, int>();
        var id = Guid.NewGuid();
        var node1 = new GraphNode<string, int>(id);
        var node2 = new GraphNode<string, int>(id);

        graph.AddNode(node1);
        graph.AddNode(node2);

        Assert.Single(graph.Nodes);
    }

    [Fact]
    public void GetNode_ReturnsNode_WhenExists()
    {
        var graph = new Graph<string, int>();
        var node = new GraphNode<string, int>("A");
        graph.AddNode(node);

        var result = graph.GetNode(node.Id);

        Assert.NotNull(result);
        Assert.Equal(node.Id, result.Id);
    }

    [Fact]
    public void GetNode_ReturnsNull_WhenNotExists()
    {
        var graph = new Graph<string, int>();

        var result = graph.GetNode(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public void AddEdge_CreatesEdge_WhenFromExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);

        graph.AddEdge(a, b, 5);

        var edge = a.Edges.Single();
        Assert.Equal(b.Id, edge.To.Id);
        Assert.Equal(5, edge.Value);
    }

    [Fact]
    public void AddEdge_DoesNothing_WhenFromNotInGraph()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");

        graph.AddEdge(a, b, 5);

        Assert.Empty(a.Edges);
    }

    [Fact]
    public void AddEdge_AllowsEdge_WhenToNotInGraph()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);

        graph.AddEdge(a, b, 5);

        Assert.Single(a.Edges);
    }

    [Fact]
    public void EditEdge_UpdatesValue_WhenEdgeExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddEdge(a, b, 5);

        graph.EditEdge(a, b, 99);

        Assert.Equal(99, a.Edges.Single().Value);
    }

    [Fact]
    public void EditEdge_DoesNothing_WhenFromNotInGraph()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");

        var ex = Record.Exception(() => graph.EditEdge(a, b, 99));

        Assert.Null(ex);
    }

    [Fact]
    public void RemoveEdge_RemovesEdge_WhenExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);
        graph.AddEdge(a, b, 5);

        graph.RemoveEdge(a, b);

        Assert.Empty(a.Edges);
    }

    [Fact]
    public void RemoveEdge_DoesNothing_WhenFromNotInGraph()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");

        var ex = Record.Exception(() => graph.RemoveEdge(a, b));

        Assert.Null(ex);
    }

    [Fact]
    public void RemoveEdge_DoesNotThrow_WhenEdgeNotExists()
    {
        var graph = new Graph<string, int>();
        var a = new GraphNode<string, int>("A");
        var b = new GraphNode<string, int>("B");
        graph.AddNode(a);
        graph.AddNode(b);

        var ex = Record.Exception(() => graph.RemoveEdge(a, b));

        Assert.Null(ex);
    }
}