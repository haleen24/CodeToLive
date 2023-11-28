using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Set : INode
{
    public IReadOnlyList<INode> Elements { get; }

    public Set(IEnumerable<INode> elements)
    {
        Elements = INode.Copy(elements);
    }

    public IEnumerable<INode?> Walk()
    {
        return Elements;
    }

    public override string ToString()
    {
        return $"Set([{string.Join(", ", Elements)}])";
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 6);

        return new Set((parser[2] as ExpressionSequence)!.Expressions);
    }
}