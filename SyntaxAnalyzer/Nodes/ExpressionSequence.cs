using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ExpressionSequence : INode  // В финальном ast быть не должно
{
    public IReadOnlyList<INode> Expressions { get; }

    public ExpressionSequence(IEnumerable<INode> expressions)
    {
        Expressions = INode.Copy(expressions);
    }

    public IEnumerable<INode?> Walk()
    {
        return Expressions;
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public static INode Construct(IParser parser)
    {
        return new ExpressionSequence(Extract(parser));
    }
}