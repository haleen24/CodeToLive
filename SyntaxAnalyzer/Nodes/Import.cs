namespace SyntaxAnalyzer.Nodes;

public class Import : INode
{
    public INode Value { get; }

    public Import(INode value)
    {
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }
}