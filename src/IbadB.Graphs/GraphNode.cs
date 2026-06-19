using System.Numerics;

namespace IbadB.Graphs;

public class GraphNode<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
	private readonly Dictionary<Guid, GraphEdge<TModel, TValue>> _edges;

    public Guid Id { get; private set; }

	public IReadOnlyCollection<GraphEdge<TModel, TValue>> Edges => _edges.Values;

	public TModel Model { get; set; }

	public GraphNode() : this(Guid.CreateVersion7(), default) { }

	public GraphNode(Guid id) : this(id, default) { }

	public GraphNode(TModel model) : this(Guid.CreateVersion7(), model) { }

	public GraphNode(Guid id, TModel model)
	{
		Id = id;
		Model = model;
		_edges = new();
    }

	public void AddEdge(GraphNode<TModel, TValue> node, TValue value) => _edges.TryAdd(node.Id, new(node, value));

    public void EditEdge(GraphNode<TModel, TValue> node, TValue value)
	{
        if (_edges.TryGetValue(node.Id, out var edge)) edge.Value = value;
    }

    public void RemoveEdge(Guid id) => _edges.Remove(id);

    public bool Equals(GraphNode<TModel, TValue>? other) => other is not null && Id == other.Id;

    public override bool Equals(object? obj) => obj is GraphNode<TModel, TValue> other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();
}