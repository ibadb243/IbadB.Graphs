using System.Numerics;

namespace IbadB.Graphs;

public class Graph<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    private readonly Dictionary<Guid, GraphNode<TModel, TValue>> _nodes;

    public IReadOnlyCollection<Guid> Nodes => _nodes.Keys;

    public Graph()
	{
        _nodes = new();
    }

    public virtual void AddNode(GraphNode<TModel, TValue> node)
    {
        if (_nodes.ContainsKey(node.Id)) return;
        _nodes.Add(node.Id, node);
    }

    public virtual GraphNode<TModel, TValue>? GetNode(Guid id)
    {
        return _nodes.TryGetValue(id, out var node) ? node : null;
    }

    public virtual void AddEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (!_nodes.ContainsKey(from.Id)) return;
        from.AddEdge(to, value);
    }

    public virtual void EditEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (!_nodes.ContainsKey(from.Id)) return;
        from.EditEdge(to, value);
    }

    public virtual void RemoveEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to)
    {
        if (!_nodes.ContainsKey(from.Id) && !_nodes.ContainsKey(to.Id)) return;
        from.RemoveEdge(to);
    }
}