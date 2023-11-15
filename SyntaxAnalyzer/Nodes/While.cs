using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class While : INode
{
    public INode Condition { get; }
    public INode Body { get; }

    public While(INode condition, INode body)
    {
        Condition = condition;
        Body = body;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Condition;
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}