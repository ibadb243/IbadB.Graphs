using System.Numerics;

namespace IbadB.Graphs;

public class Pathway<TModel, TValue> where TModel : class where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
{
    public IList<GraphNode<TModel, TValue>> Vertexes { get; set; }
}