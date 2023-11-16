using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Import : INode
{
    public INode Value { get; }

    public Import(INode value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Import(module={Value})";
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