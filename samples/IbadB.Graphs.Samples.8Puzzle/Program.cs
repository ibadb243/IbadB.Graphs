using IbadB.Graphs;
using IbadB.Graphs.Samples._8Puzzle;

var graph = new GameGraph();

var init = new GraphNode<StateModel, int>(new StateModel
{
    Field = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 }
});

var goal = new GraphNode<StateModel, int>(new StateModel
{
    Field = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }
});

graph.AddNodeIndexed(init);
graph.AddNodeIndexed(goal);

Pathway<StateModel, int> path = graph.AStar(init, goal, HeuresticFunction);

foreach (var p in path.Vertexes)
{
    Draw(p.Model);
    Console.WriteLine();
}
Console.WriteLine(path.Vertexes.Count);

Console.ReadKey();

int HeuresticFunction(GraphNode<StateModel, int> node)
{
    StateModel s = node.Model;
    HashSet<byte> nums = new HashSet<byte>(goal.Model.Field);

    int size = (int)Math.Sqrt(nums.Count);
    int h = 0;

    foreach (byte num in nums)
    {
        int index_G = Array.IndexOf(goal.Model.Field, num);
        int index_S = Array.IndexOf(node.Model.Field, num);

        h += Math.Abs(index_S % size - index_G % size) + Math.Abs(index_S / size - index_G / size);
    }

    return h;
}

void Draw(StateModel state)
{
    int size = (int)Math.Sqrt(state.Field.Length);

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            Console.Write($"{state.Field[i * size + j]} ");
        }
        Console.WriteLine(/*$"{state.Field[i * size]} {state.Field[i * size + 1]} {state.Field[i * size + 2]}"*/);
    }

    //Console.WriteLine($"{state.Field[0]} {state.Field[1]} {state.Field[2]}");
    //Console.WriteLine($"{state.Field[3]} {state.Field[4]} {state.Field[5]}");
    //Console.WriteLine($"{state.Field[6]} {state.Field[7]} {state.Field[8]}");
}
