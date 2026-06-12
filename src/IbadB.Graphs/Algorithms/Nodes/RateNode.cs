using System.Numerics;

namespace IbadB.Graphs.Algorithms.Nodes;

public class RateNode<TModel, TValue>
    where TModel : class
    where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    public TValue T { get; set; }
    public GraphNode<TModel, TValue> Node { get; set; }

    public RateNode(GraphNode<TModel, TValue> node, TValue t)
    {
        T = t;
        Node = node;
    }
}
