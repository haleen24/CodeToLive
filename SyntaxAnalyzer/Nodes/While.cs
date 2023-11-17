using System.Diagnostics;
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

    public override string ToString()
    {
        return $"While(condition={Condition}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Condition;
        yield return Body;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 5);
        return new While(parser[2], parser[4]);
    }
}