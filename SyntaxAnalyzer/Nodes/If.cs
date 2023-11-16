using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class If : INode
{
    public INode Condition { get; }
    public INode Body { get; }
    public INode? Else { get; }

    public If(INode condition, INode body, INode? @else = null)
    {
        Condition = condition;
        Body = body;
        Else = @else;
    }

    public override string ToString()
    {
        return $"If(condition={Condition}, body={Body}, else={Else})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Condition;
        yield return Body;
        yield return Else;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}