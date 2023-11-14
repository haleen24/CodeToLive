namespace SyntaxAnalyzer.Nodes;

public class Return : INode
{
    public INode? Value { get; }

    public Return(INode value)
    {
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }
}