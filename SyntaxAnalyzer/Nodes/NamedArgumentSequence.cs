namespace SyntaxAnalyzer.Nodes;

public class NamedArgumentSequence : INode // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> NamedArguments { get; }

    public NamedArgumentSequence(IEnumerable<INode> namedArguments)
    {
        NamedArguments = new List<INode>(namedArguments).AsReadOnly();
    }

    public IEnumerable<INode?> Walk()
    {
        return NamedArguments;
    }
}