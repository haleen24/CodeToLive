using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class LambdaExpression : INode
{
    public IReadOnlyList<INode> Arguments { get; }
    public INode Body { get; }

    public LambdaExpression(IEnumerable<INode> arguments, INode body)
    {
        Arguments = INode.Copy(arguments);
        Body = body;
    }

    public override string ToString()
    {
        return
            $"LambdaExpression(arguments=[{string.Join(", ", Arguments)}], body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        foreach (INode node in Arguments)
        {
            yield return node;
        }

        yield return Body;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 7);

        var args = parser[2] as FormalArguments;
        return args switch
        {
            { } nnargs => new LambdaExpression(nnargs.Arguments, parser[^1]),
            _ => new LambdaExpression(new List<INode>(), parser[^1])
        };
    }
}