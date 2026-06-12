using System.Numerics;

namespace IbadB.Graphs.Algorithms.Nodes;

internal class HeuristicNode<TModel, TValue>
    where TModel : class
    where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>, ISubtractionOperators<TValue, TValue, TValue>
{
    public TValue T { get; set; }
    public TValue H { get; set; }

    public GraphNode<TModel, TValue> Node { get; set; }

    public HeuristicNode(GraphNode<TModel, TValue> node, TValue t, TValue h)
    {
        T = t;
        H = h;
        Node = node;
    }
}
