using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Break : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }

    public static INode Construct(IParser parser)
    {
        return new Break();
    }
}