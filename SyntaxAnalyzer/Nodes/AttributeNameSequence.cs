using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class AttributeNameSequence : INode
{
    public IReadOnlyList<INode> AttributeNames { get; }

    public AttributeNameSequence(IEnumerable<INode> attributeNames)
    {
        AttributeNames = INode.Copy(attributeNames);
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public IEnumerable<INode?> Walk()
    {
        return AttributeNames;
    }

    public static INode Construct(IParser parser)
    {
        return new AttributeNameSequence(Extract(parser));
    }
}