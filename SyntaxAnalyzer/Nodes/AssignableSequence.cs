namespace SyntaxAnalyzer.Nodes;

public class
    AssignableSequence : INode // Используется для AssignableSequence,
    // AdditionalAssignables, OptionalAdditionalAssignables, в итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Assignables { get; }

    public AssignableSequence(IReadOnlyList<INode> assignables)
    {
        Assignables = assignables;
    }

    public IEnumerable<INode> Walk()
    {
        return Assignables;
    }
}