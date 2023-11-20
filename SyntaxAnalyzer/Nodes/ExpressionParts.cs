using System.Collections;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ExpressionParts : INode
{
    public IReadOnlyList<INode> Parts { get; }

    public ExpressionParts(IEnumerable<INode> parts)
    {
        Parts = INode.Copy(parts);
    }

    public IEnumerable<INode?> Walk()
    {
        return Parts;
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; ++i)
        {
            yield return parser[i];
        }
    }

    public static INode Construct(IParser parser)
    {
        return new ExpressionParts(Extract(parser));
    }
}