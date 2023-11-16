using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Attribute : INode
{
    public INode AttributeOf { get; }
    public INode AttributeName { get; }

    public Attribute(INode attributeOf, INode attributeName)
    {
        AttributeOf = attributeOf;
        AttributeName = attributeName;
    }

    public override string ToString()
    {
        return $"Attribute(of={AttributeOf}, name={AttributeName})";
    }

    public IEnumerable<INode> Walk()
    {
        yield return AttributeOf;
        yield return AttributeName;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}