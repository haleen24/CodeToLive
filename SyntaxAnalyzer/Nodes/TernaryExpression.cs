using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class TernaryExpression : INode
{
    public INode Condition { get; }
    public INode FirstBranch { get; }
    public INode SecondBranch { get; }

    public TernaryExpression(INode condition, INode firstBranch, INode secondBranch)
    {
        Condition = condition;
        FirstBranch = firstBranch;
        SecondBranch = secondBranch;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Condition;
        yield return FirstBranch;
        yield return SecondBranch;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}