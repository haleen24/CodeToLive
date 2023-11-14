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

    public IEnumerable<INode> Walk()
    {
        yield return AttributeOf;
        yield return AttributeName;
    }
}