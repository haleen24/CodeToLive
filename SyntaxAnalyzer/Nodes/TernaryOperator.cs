using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class TernaryOperator : INode
{
    public INode FirstBranch { get; }
    public INode SecondBranch { get; }

    public TernaryOperator(INode firstBranch, INode secondBranch)
    {
        FirstBranch = firstBranch;
        SecondBranch = secondBranch;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return FirstBranch;
        yield return SecondBranch;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        return new TernaryOperator(parser[1], parser[3]);
    }
}