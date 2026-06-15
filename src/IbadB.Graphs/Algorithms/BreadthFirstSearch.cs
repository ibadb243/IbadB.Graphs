using System.Numerics;

namespace IbadB.Graphs.Algorithms;

public static partial class AlgorithmExtensions
{
    public static Pathway<TModel, TValue> BFS<TModel, TValue>(this Graph<TModel, TValue> graph, GraphNode<TModel, TValue> start, GraphNode<TModel, TValue> end)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    {
        Queue<GraphNode<TModel, TValue>> queue = new();
        HashSet<GraphNode<TModel, TValue>> enqueued = new();
        Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path = new();

        queue.Enqueue(start);
        enqueued.Add(start);

        GraphNode<TModel, TValue> current;
        while (queue.Count > 0)
        {
            current = queue.Dequeue();

            if (current.Id == end.Id) break;

            foreach (var edge in current.Edges)
            {
                if (enqueued.Contains(edge.To)) continue;

                enqueued.Add(edge.To);
                queue.Enqueue(edge.To);
                path.Add(edge.To, current);
            }
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(path, start, end) };
    }

    private static IList<GraphNode<TModel, TValue>> _GetPath<TModel, TValue>(Dictionary<GraphNode<TModel, TValue>, GraphNode<TModel, TValue>> path, GraphNode<TModel, TValue> start, GraphNode<TModel, TValue> end)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    {
        List<GraphNode<TModel, TValue>> result = new List<GraphNode<TModel, TValue>>(path.Count + 1) { end };

        GraphNode<TModel, TValue> current = end;
        while (path.TryGetValue(current, out var next))
        {
            result.Add(next);
            if (next.Id == start.Id) break;
            current = next;
        }

        result.Reverse();
        return result;
    }
}
