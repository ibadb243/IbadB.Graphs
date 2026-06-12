using System.Numerics;
using System.Xml.Linq;

namespace IbadB.Graphs;

public class Graph<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    public Dictionary<GraphNode<TModel, TValue>, IList<GraphEdge<TModel, TValue>>> Nodes{ get; set; }

	public Graph()
	{
        Nodes = new();
    }

    public virtual void AddNode(GraphNode<TModel, TValue> node)
    {
        if (Nodes.Keys.Any(n => n.Id == node.Id)) return;
        Nodes.Add(node, node.Edges);
    }

    public virtual void AddEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (!Nodes.Keys.Any(n => n.Id == from.Id)) return;
        from.AddEdge(to, value);
        //Nodes[from].Add(new GraphEdge<TModel, TValue>(from, to, value));
    }

    public virtual void EditEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        if (!Nodes.Keys.Any(n => n.Id == from.Id)) return;
        from.EditEdge(to, value);
    }

    public virtual void RemoveEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to)
    {
        if (!Nodes.Keys.Any(n => n.Id == from.Id) && !Nodes.Keys.Any(n => n.Id == to.Id)) return;
        from.RemoveEdge(to);
    }

    // public void Save(string path)

    // public void Load(string pathNodes, string PathEdges)
}