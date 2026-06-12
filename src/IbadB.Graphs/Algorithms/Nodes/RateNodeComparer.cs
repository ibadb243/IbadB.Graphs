using System.Numerics;

namespace IbadB.Graphs.Algorithms.Nodes;

internal class RateNodeComparer<TModel, TValue> : IComparer<RateNode<TModel, TValue>>
    where TModel : class
    where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    public int Compare(RateNode<TModel, TValue>? x, RateNode<TModel, TValue>? y)
    {
        return x.T.CompareTo(y.T) == 0 ? x.Node.Id.CompareTo(y.Node.Id) : x.T.CompareTo(y.T);
    }
}
