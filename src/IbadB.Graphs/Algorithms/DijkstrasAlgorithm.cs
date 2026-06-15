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
        PriorityQueue<GraphNode<TModel, TValue>, TValue> queue = new();
        HashSet<GraphNode<TModel, TValue>> visited = new();
        Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path = new();
        Dictionary<GraphNode<TModel, TValue>, TValue> distances = new();

        queue.Enqueue(start, default);
        distances[start] = default;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (current.Id == end.Id) break;

            foreach (var edge in current.Edges)
            {
                if (visited.Contains(edge.To)) continue;

                var newDistance = distances[current] + edge.Value;

                if (!distances.ContainsKey(edge.To) || newDistance.CompareTo(distances[edge.To]) < 0)
                {
                    distances[edge.To] = newDistance;
                    path[edge.To] = current;

                    queue.Enqueue(edge.To, newDistance);
                }
            }
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(path, start, end) };
    }

    //public static Pathway<TModel, TValue> Dijkstra<TModel, TValue>(
    //    this Graph<TModel, TValue> graph,
    //    GraphNode<TModel, TValue> start,
    //    GraphNode<TModel, TValue> end)
    //    where TModel : class
    //    where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    //{
    //    SortedSet<RateNode<TModel, TValue>> set = new(new RateNodeComparer<TModel, TValue>()) { new(start, default) };
    //    Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path = new();

    //    RateNode<TModel, TValue> current;
    //    while (set.Count > 0)
    //    {
    //        current = set.Min;
    //        set.Remove(current);

    //        if (current.Node.Id == end.Id) break;

    //        foreach (var edge in current.Node.Edges)
    //        {
    //            if (path.ContainsKey(edge.To)) continue;

    //            set.Add(new (edge.To, current.T + edge.Value));
    //            path.Add(edge.To, current.Node);
    //        }
    //    }

    //    return new Pathway<TModel, TValue>() { Vertexes = _GetPath(path, start, end) };
    //}
}
