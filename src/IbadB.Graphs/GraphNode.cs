using System.Numerics;

namespace IbadB.Graphs;

public class GraphNode<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
	private static int _id = 0;

	public Guid Id { get; private set; }
	public string Name { get; set; }
	public IList<GraphEdge<TModel, TValue>> Edges { get; set; }

	public TModel Model { get; set; }

	public GraphNode()
		: this(Guid.NewGuid(), $"GraphNode ({++_id})", new List<GraphEdge<TModel, TValue>>(), default) { }

	public GraphNode(Guid id)
		: this(id, $"GraphNode ({++_id})", new List<GraphEdge<TModel, TValue>>(), default) { }

	public GraphNode(string name, TModel model)
		: this(Guid.NewGuid(), name, new List<GraphEdge<TModel, TValue>>(), model) { }

	public GraphNode(Guid id, string name, IList<GraphEdge<TModel, TValue>> edges, TModel model)
	{
		Id = id;
		Name = name;
		Edges = edges;
		Model = model;
	}

	public void AddEdge(GraphNode<TModel, TValue> node, TValue value)
	{
        if (Edges.Any(e => e.To.Id == node.Id)) return;
        Edges.Add(new GraphEdge<TModel, TValue>(this, node, value));
    }

	public void EditEdge(GraphNode<TModel, TValue> node, TValue value)
	{
		if (!Edges.Any(e => e.To.Id == node.Id)) return;
		Edges.First(e => e.To.Id == node.Id).Value = value;
	}

	public void RemoveEdge(GraphNode<TModel,TValue> node)
	{
		var edge = Edges.First(e => e.To.Id == node.Id);
		Edges.Remove(edge);
	}
	public void RemoveEdge(Guid id)
	{
		var edge = Edges.First(e => e.To.Id == id);
		Edges.Remove(edge);
	}
}