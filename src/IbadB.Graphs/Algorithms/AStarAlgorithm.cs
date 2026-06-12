using System.Numerics;
using IbadB.Graphs.Algorithms.Nodes;

namespace IbadB.Graphs.Algorithms;

public static partial class AlgorithmExtensions
{
    public static Pathway<TModel, TValue> AStar<TModel, TValue>(
        this Graph<TModel, TValue> graph,
        GraphNode<TModel, TValue> start,
        GraphNode<TModel, TValue> end,
        Func<GraphNode<TModel, TValue>, TValue> h)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>, ISubtractionOperators<TValue, TValue, TValue>
    {
        SortedSet<HeuristicNode<TModel, TValue>> set = new SortedSet<HeuristicNode<TModel, TValue>>(new HeuristicNodeComparer<TModel, TValue>()) { new(start, default, default) };
        Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path = new Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>>();

        HeuristicNode<TModel, TValue> current;
        while (set.Count > 0)
        {
            current = set.Min;
            set.Remove(current);

            if (current.Node.Id == end.Id) break;

            foreach (var edge in current.Node.Edges)
            {
                if (path.ContainsKey(edge.To)) continue;

                set.Add(new(edge.To, current.T + edge.Value + h(edge.To), h(edge.To)));
                path.Add(edge.To, current.Node);
            }
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(path, start, end) };
    }
}
