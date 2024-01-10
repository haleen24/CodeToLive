using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public static class ExpressionConstructor
{
    public static INode ConstructExpressionWithoutBinaryOperators(IParser parser)
    {
        Debug.Assert(parser.Length == 2);

        INode res = parser[0];

        if (parser[1] is ExpressionParts ep)
        {
            foreach (INode part in ep.Parts)
            {
                res = part switch
                {
                    Idle => new FunctionCall(res, new List<INode>()),
                    ActualArguments args => new FunctionCall(res, args.Arguments),
                    IndexatorPart ip => new Indexator(res, ip.Expr),
                    AttributePart ap => new Attribute(res, ap.Name),
                    _ => throw new Exception("Wrong expression part") // Никогда не должно случиться
                };
            }
        }

        return res;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);

        INode res = parser[0];

        if (parser[1] is TernaryOperator to)
        {
            res = new TernaryExpression(res, to.FirstBranch, to.SecondBranch);
        }

        return res;
    }
}