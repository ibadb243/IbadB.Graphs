using System.Numerics;

namespace IbadB.Graphs.Algorithms;

public static partial class AlgorithmExtensions
{
    public static Pathway<TModel, TValue> DFS<TModel, TValue>(this Graph<TModel, TValue> graph, GraphNode<TModel, TValue> start, GraphNode<TModel, TValue> end)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue>
    {
        Stack<GraphNode<TModel, TValue>> stack = new Stack<GraphNode<TModel, TValue>>();
        HashSet<GraphNode<TModel, TValue>> visited = new HashSet<GraphNode<TModel, TValue>>();

        stack.Push(start);

        GraphNode<TModel, TValue> current;
        while (stack.Count > 0)
        {
            current = stack.Peek();
            visited.Add(current);

            if (current.Id == end.Id) break;

            var next = current.Edges
                .Select(e => e.To)
                .FirstOrDefault(n => !visited.Contains(n));

            if (next is null)
                stack.Pop();
            else
                stack.Push(next);
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(stack) };
    }

    private static IList<GraphNode<TModel, TValue>> _GetPath<TModel, TValue>(Stack <GraphNode<TModel, TValue>> stack)
        where TModel : class
        where TValue : IComparable, IAdditionOperators<TValue, TValue, TValue> => stack.Reverse().ToList();
}
