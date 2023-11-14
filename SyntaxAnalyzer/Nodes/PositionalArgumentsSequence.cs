namespace SyntaxAnalyzer.Nodes;

public class PositionalArgumentsSequence : INode  // Можно исползовать и для формальных, и для фактических
                                                  // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Arguments { get; }

    public PositionalArgumentsSequence(IEnumerable<INode> arguments)
    {
        Arguments = new List<INode>(arguments).AsReadOnly();
    }

    public IEnumerable<INode?> Walk()
    {
        return Arguments;
    }
}