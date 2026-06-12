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
        Stack<GraphNode<TModel, TValue>> stack = new Stack<GraphNode<TModel, TValue>>();
        HashSet<GraphNode<TModel, TValue>> visited = new HashSet<GraphNode<TModel, TValue>>();

        stack.Push(start);

        GraphNode<TModel, TValue> current;
        IEnumerable<GraphNode<TModel, TValue>> children;
        while (stack.Count > 0)
        {
            current = stack.Peek();
            visited.Add(current);

            if (current == end) break;
            children = current.Edges.Select(e => e.To).Where(n => !visited.Contains(n)).OrderBy(n => h(n));
            if (children.Count() == 0) stack.Pop();
            else stack.Push(children.First());
        }

        return new Pathway<TModel, TValue>() { Vertexes = _GetPath(stack) };
    }
}
