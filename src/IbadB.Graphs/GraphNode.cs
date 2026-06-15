using System.Numerics;

namespace IbadB.Graphs;

public class GraphNode<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
	private static int _id = 0;

	private readonly Dictionary<Guid, GraphEdge<TModel, TValue>> _edges;

    public Guid Id { get; private set; }
	public string Name { get; set; }
	public IReadOnlyCollection<GraphEdge<TModel, TValue>> Edges => _edges.Values;

	public TModel Model { get; set; }

	public GraphNode() : this(Guid.CreateVersion7(), $"GraphNode ({++_id})", default) { }

	public GraphNode(Guid id) : this(id, $"GraphNode ({++_id})", default) { }

	public GraphNode(string name, TModel model) : this(Guid.CreateVersion7(), name, model) { }

	public GraphNode(Guid id, string name, TModel model)
	{
		Id = id;
		Name = name;
		Model = model;
		_edges = new Dictionary<Guid, GraphEdge<TModel, TValue>>();
    }

	public void AddEdge(GraphNode<TModel, TValue> node, TValue value)
	{
        if (_edges.ContainsKey(node.Id)) return;
		_edges.Add(node.Id, new GraphEdge<TModel, TValue>(this, node, value));
    }

	public void EditEdge(GraphNode<TModel, TValue> node, TValue value)
	{
        if (!_edges.TryGetValue(node.Id, out var edge)) return;
        edge.Value = value;
    }

	public void RemoveEdge(GraphNode<TModel,TValue> node) => _edges.Remove(node.Id);

    public void RemoveEdge(Guid id) => _edges.Remove(id);

    public bool Equals(GraphNode<TModel, TValue>? other) => other is not null && Id == other.Id;

    public override bool Equals(object? obj) => obj is GraphNode<TModel, TValue> other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();
}