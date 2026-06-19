namespace IbadB.Graphs.Samples._8Puzzle;

internal class GameGraph : Graph<StateModel, int>
{
    private readonly Dictionary<string, GraphNode<StateModel, int>> _stateIndex = new();

    private static string GetKey(byte[] field) => string.Join(",", field);

    public void AddNodeIndexed(GraphNode<StateModel, int> node)
    {
        AddNode(node);
        _stateIndex[GetKey(node.Model.Field)] = node;
    }

    public bool TryFindByState(StateModel state, out GraphNode<StateModel, int>? node) 
        => _stateIndex.TryGetValue(GetKey(state.Field), out node);

    public bool HasState(StateModel state)
    {
        throw new NotImplementedException();
        //return Nodes.
        //return Nodes.Any(id => Enumerable.SequenceEqual(state.Field, GetNode(id).Model.Field));
    }
}
