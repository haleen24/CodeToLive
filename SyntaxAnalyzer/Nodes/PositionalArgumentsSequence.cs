using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class PositionalArgumentsSequence : INode // Можно исползовать и для формальных, и для фактических
    // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Arguments { get; }

    public PositionalArgumentsSequence(IEnumerable<INode> arguments)
    {
        Arguments = INode.Copy(arguments);
    }

    public IEnumerable<INode?> Walk()
    {
        return Arguments;
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
        return new PositionalArgumentsSequence(Extract(parser));
    }
}