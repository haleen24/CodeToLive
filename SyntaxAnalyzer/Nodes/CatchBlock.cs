using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class CatchBlock : INode // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Catches { get; }
    public INode? Else { get; }
    public INode? Finally { get; }

    public CatchBlock(IEnumerable<INode> catches, INode? @else, INode? @finally)
    {
        Catches = INode.Copy(catches);
        Else = @else;
        Finally = @finally;
    }

    public IEnumerable<INode?> Walk()
    {
        foreach (INode node in Catches)
        {
            yield return node;
        }

        yield return Else;
        yield return Finally;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new CatchBlock((parser[0] as CatchSequence)!.Catches, parser[1] is Idle ? null : parser[1],
            parser[2] is Idle ? null : parser[2]);
    }
}