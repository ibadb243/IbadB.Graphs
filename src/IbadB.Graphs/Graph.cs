using System.Collections.Concurrent;
using System.Numerics;

namespace IbadB.Graphs;

public class Graph<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    private readonly IDictionary<Guid, GraphNode<TModel, TValue>> _nodes;

    public IReadOnlyDictionary<Guid, GraphNode<TModel, TValue>> Nodes => _nodes.AsReadOnly();

    public Graph(bool safeThread = true)
	{
        if (safeThread) _nodes = new ConcurrentDictionary<Guid, GraphNode<TModel, TValue>>();
        else _nodes = new Dictionary<Guid, GraphNode<TModel, TValue>>();
    }

    public virtual void AddNode(GraphNode<TModel, TValue> node) => _nodes.TryAdd(node.Id, node);

    public virtual GraphNode<TModel, TValue>? GetNode(Guid id) => _nodes.TryGetValue(id, out var node) ? node : null;

    public virtual void AddEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (_nodes.TryGetValue(from.Id, out var node)) node.AddEdge(to, value);
    }

    public virtual void EditEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (_nodes.TryGetValue(from.Id, out var node)) node.EditEdge(to, value);
    }

    public virtual void RemoveEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to)
    {
        if (_nodes.TryGetValue(from.Id, out var node)) node.RemoveEdge(to.Id);
    }
}