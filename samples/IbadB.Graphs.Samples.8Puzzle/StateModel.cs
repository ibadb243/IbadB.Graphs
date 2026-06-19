namespace IbadB.Graphs.Samples._8Puzzle;

internal class StateModel
{
    public byte[] Field { get; set; }

    public StateModel()
    {
        Field = new byte[9] { 1, 2, 3, 8, 0, 4, 7, 6, 5 };
    }

    public static List<StateModel> CreateNextStates(StateModel state)
    {
        int size = (int)Math.Sqrt(state.Field.Length);
        int index = Array.IndexOf(state.Field, (byte)0);
        List<StateModel> states = new List<StateModel>();

        if (index - size >= 0) states.Add(Swap(state, index, index - size));
        if (index + size < size * size) states.Add(Swap(state, index, index + size));
        if (index % size - 1 >= 0) states.Add(Swap(state, index, index - 1));
        if (index % size + 1 < size) states.Add(Swap(state, index, index + 1));

        return states;
    }

    public static StateModel Swap(StateModel state, int index1, int index2)
    {
        byte[] array = new byte[state.Field.Length];
        Array.Copy(state.Field, array, state.Field.Length);

        byte temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;

        return new StateModel() { Field = array };
    }
}
