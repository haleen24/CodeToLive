using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Else : INode  // В итоговом дереве быть не должно
{
    public INode Body { get; }

    public Else(INode body)
    {
        Body = body;
    }

    public IEnumerable<INode> Walk()
    {
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}