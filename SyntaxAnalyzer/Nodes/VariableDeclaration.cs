using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class IdentifierWithFinal : INode  // IdentifierWithFinal
{
    public string Value { get; }

    public IdentifierWithFinal(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"IdentifierWithFinal(name={Value})";
    }

    public IEnumerable<INode> Walk()
    {
        yield break;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}