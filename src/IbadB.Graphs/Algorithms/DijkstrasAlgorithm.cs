using System.Numerics;
using IbadB.Graphs.Algorithms.Nodes;

namespace IbadB.Graphs.Algorithms;

public static partial class AlgorithmExtensions
{
    public static Pathway<TModel, TValue> Dijkstra<TModel, TValue>(
        this Graph<TModel, TValue> graph,
        GraphNode<TModel, TValue> start,
        GraphNode<TModel, TValue> end)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    {
        SortedSet<RateNode<TModel, TValue>> set = new SortedSet<RateNode<TModel, TValue>>(new RateNodeComparer<TModel, TValue>()) { new(start, default) };
        Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path = new Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>>();

        RateNode<TModel, TValue> current;
        while (set.Count > 0)
        {
            current = set.Min;
            set.Remove(current);

            if (current.Node.Id == end.Id) break;

            foreach (var edge in current.Node.Edges)
            {
                if (path.ContainsKey(edge.To)) continue;

                set.Add(new (edge.To, current.T + edge.Value));
                path.Add(edge.To, current.Node);
            }
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(path, start, end) };
    }
}
