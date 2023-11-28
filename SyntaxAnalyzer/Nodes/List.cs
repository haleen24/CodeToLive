using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class List : INode
{
    public IReadOnlyList<INode> Elements { get; }

    public List(IEnumerable<INode> elements)
    {
        Elements = INode.Copy(elements);
    }

    public IEnumerable<INode?> Walk()
    {
        return Elements;
    }

    public override string ToString()
    {
        return $"List([{string.Join(", ", Elements)}])";
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 6);

        return parser[2] switch
        {
            Idle => new List(new List<INode>()),
            ExpressionSequence seq => new List(seq.Expressions),
            _ => throw new Exception("Wrong sequence for list") // Никогда не должно случиться
        };
    }
}