using System.Numerics;

namespace IbadB.Graphs.Algorithms;

public static partial class AlgorithmExtensions
{
    public static Pathway<TModel, TValue> GBFS<TModel, TValue>(
        this Graph<TModel, TValue> graph,
        GraphNode<TModel, TValue> start,
        GraphNode<TModel, TValue> end,
        Func<GraphNode<TModel, TValue>, TValue> h)
            where TModel : class
            where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    {
        Stack<GraphNode<TModel, TValue>> stack = new();
        HashSet<GraphNode<TModel, TValue>> visited = new();

        stack.Push(start);

        GraphNode<TModel, TValue> current;
        while (stack.Count > 0)
        {
            current = stack.Peek();
            visited.Add(current);

            if (current.Id == end.Id) break;

            var next = current.Edges
                .Select(e => e.To)
                .Where(n => !visited.Contains(n))
                .OrderBy(n => h(n))
                .FirstOrDefault();

            if (next is null)
                stack.Pop();
            else
                stack.Push(next);
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(stack) };
    }
}
