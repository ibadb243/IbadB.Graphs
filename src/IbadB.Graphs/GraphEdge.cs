using System.Numerics;

namespace IbadB.Graphs;

public class GraphEdge<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    public GraphNode<TModel, TValue> From { get; set; }
    public GraphNode<TModel, TValue> To { get; set; }
    public TValue Value { get; set; }

    public GraphEdge(GraphNode<TModel, TValue> from, GraphNode<TModel, TValue> to, TValue value)
    {
        From = from;
        To = to;
        Value = value;
    }
}