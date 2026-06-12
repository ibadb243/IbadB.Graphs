using System.Numerics;

namespace IbadB.Graphs.Algorithms.Nodes;

internal class HeuristicNodeComparer<TModel, TValue> : IComparer<HeuristicNode<TModel, TValue>>
    where TModel : class
    where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>, ISubtractionOperators<TValue, TValue, TValue>
{
    public int Compare(HeuristicNode<TModel, TValue>? x, HeuristicNode<TModel, TValue>? y)
    {
        return (x.T - x.H).CompareTo(y.T - y.H) == 0 ? x.Node.Id.CompareTo(y.Node.Id) : (x.T - x.H).CompareTo(y.T - y.H);
    }
}
