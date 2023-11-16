using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Comma : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}