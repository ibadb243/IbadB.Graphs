namespace IbadB.Graphs.Tests;

public class GraphNodeTests
{
    [Fact]
    public void AddEdge_CreatesEdge()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");

        a.AddEdge(b, 5);

        var edge = a.Edges.Single();
        Assert.Equal(b.Id, edge.To.Id);
        Assert.Equal(5, edge.Value);
    }

    [Fact]
    public void AddEdge_Duplicate_IsIgnored()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");

        a.AddEdge(b, 5);
        a.AddEdge(b, 10);

        Assert.Single(a.Edges);
        Assert.Equal(5, a.Edges.Single().Value);
    }

    [Fact]
    public void EditEdge_UpdatesValue()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        a.AddEdge(b, 5);

        a.EditEdge(b, 99);

        Assert.Equal(99, a.Edges.Single().Value);
    }

    [Fact]
    public void EditEdge_NonExistent_DoesNothing()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");

        a.EditEdge(b, 99);

        Assert.Empty(a.Edges);
    }

    [Fact]
    public void RemoveEdge_RemovesExistingEdge()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        a.AddEdge(b, 5);

        a.RemoveEdge(b);

        Assert.Empty(a.Edges);
    }

    [Fact]
    public void RemoveEdge_NonExistent_DoesNotThrow()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");

        var ex = Record.Exception(() => a.RemoveEdge(b));

        Assert.Null(ex);
    }

    [Fact]
    public void RemoveEdge_ByGuid_RemovesExistingEdge()
    {
        var a = new GraphNode<string, int>("A", "A");
        var b = new GraphNode<string, int>("B", "B");
        a.AddEdge(b, 5);

        a.RemoveEdge(b.Id);

        Assert.Empty(a.Edges);
    }

    [Fact]
    public void Equals_NodesWithSameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var node1 = new GraphNode<string, int>(id);
        var node2 = new GraphNode<string, int>(id);

        Assert.True(node1.Equals(node2));
        Assert.Equal(node1.GetHashCode(), node2.GetHashCode());
    }

    [Fact]
    public void Equals_NodesWithDifferentId_AreNotEqual()
    {
        var node1 = new GraphNode<string, int>(Guid.NewGuid());
        var node2 = new GraphNode<string, int>(Guid.NewGuid());

        Assert.False(node1.Equals(node2));
    }
}