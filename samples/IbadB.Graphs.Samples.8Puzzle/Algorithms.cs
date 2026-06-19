using System.Numerics;

namespace IbadB.Graphs.Samples._8Puzzle;

internal static class Algorithms
{
    public static Pathway<StateModel, int> AStar(
        this GameGraph graph,
        GraphNode<StateModel, int> start,
        GraphNode<StateModel, int> goal,
        Func<GraphNode<StateModel, int>, int> h)
    {
        var queue = new PriorityQueue<GraphNode<StateModel, int>, int>();
        var visited = new HashSet<GraphNode<StateModel, int>>();
        var path = new Dictionary<GraphNode<StateModel, int>, GraphNode<StateModel, int>>();
        var distances = new Dictionary<GraphNode<StateModel, int>, int>();

        queue.Enqueue(start, 0);
        distances[start] = 0;

        GraphNode<StateModel, int>? foundGoal = null;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (Enumerable.SequenceEqual(current.Model.Field, goal.Model.Field))
            {
                foundGoal = current;
                break;
            }

            var states = StateModel.CreateNextStates(current.Model);
            foreach (var state in states)
            {
                if (graph.TryFindByState(state, out var node))
                {
                    graph.AddEdge(current, node, 1);
                }
                else
                {
                    node = new GraphNode<StateModel, int>() { Model = state };
                    graph.AddNodeIndexed(node);
                    graph.AddEdge(current, node, 1);
                }
            }

            foreach (var edge in current.Edges)
            {
                if (visited.Contains(edge.To)) continue;

                var newDistance = distances[current] + edge.Value;

                if (!distances.ContainsKey(edge.To) || newDistance.CompareTo(distances[edge.To]) < 0)
                {
                    distances[edge.To] = newDistance;
                    path[edge.To] = current;

                    queue.Enqueue(edge.To, newDistance + h(edge.To));
                }
            }
        }

        return new Pathway<StateModel, int>() { Vertexes = GetPath(path, start, foundGoal ?? goal) };
    }

    private static IList<GraphNode<StateModel, int>> GetPath(Dictionary<GraphNode<StateModel, int>, GraphNode<StateModel, int>> path, GraphNode<StateModel, int> start, GraphNode<StateModel, int> end)
    {
        List<GraphNode<StateModel, int>> result = new List<GraphNode<StateModel, int>>() { end };

        GraphNode<StateModel, int> current = end;
        while (path.TryGetValue(current, out GraphNode<StateModel, int> next))
        {
            result.Add(next);
            if (next.Id == start.Id) break;
            current = next;
        }

        result.Reverse();
        return result;
    }
}
