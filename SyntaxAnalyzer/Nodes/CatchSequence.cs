using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class CatchSequence : INode // в итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Catches { get; }

    public CatchSequence(IEnumerable<INode> catches)
    {
        Catches = INode.Copy(catches);
    }

    public IEnumerable<INode?> Walk()
    {
        return Catches;
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
        return new CatchSequence(Extract(parser));
    }
}