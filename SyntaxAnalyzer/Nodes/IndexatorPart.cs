using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class IndexatorPart : INode
{
    public INode Expr { get; }

    public IndexatorPart(INode expr)
    {
        Expr = expr;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Expr;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 5);
        return new IndexatorPart(parser[2]);
    }
}