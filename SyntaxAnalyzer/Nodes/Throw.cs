using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Throw : INode
{
    public INode Value { get; }

    public Throw(INode value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Throw(exception={Value})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}