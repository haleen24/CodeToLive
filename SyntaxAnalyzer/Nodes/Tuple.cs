using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Tuple : INode
{
    public IReadOnlyList<INode> Elements { get; }

    public Tuple(IEnumerable<INode> elements)
    {
        Elements = INode.Copy(elements);
    }

    public IEnumerable<INode?> Walk()
    {
        return Elements;
    }

    public override string ToString()
    {
        return $"Tuple([{string.Join(", ", Elements)}])";
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        yield return parser[0];

        if (parser[3] is ExpressionSequence seq)
        {
            foreach (INode node in seq.Expressions)
            {
                yield return node;
            }
        }
    }

    public static INode ConstructBody(IParser parser)
    {
        Debug.Assert(parser.Length == 5);

        return new Tuple(Extract(parser));
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 5);

        return parser[2] switch
        {
            Idle => new Tuple(new List<INode>()),
            Tuple t => t,
            _ => throw new Exception("Wrong sequence for tuple") // Никогда не должно случиться
        };
    }
}