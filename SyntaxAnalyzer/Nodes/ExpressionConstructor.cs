using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public static class ExpressionConstructor
{
    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);

        INode res = parser[0];

        if (parser[1] is ExpressionParts ep)
        {
            foreach (INode part in ep.Parts)
            {
                res = part switch
                {
                    Idle => new FunctionCall(res, new List<INode>(), new List<INode>()),
                    Arguments args => new FunctionCall(res, args.Positional, args.Named, args.Params),
                    IndexatorPart ip => new Indexator(res, ip.Expr),
                    AttributePart ap => new Attribute(res, ap.Name),
                    _ => throw new Exception("Wrong expression part") // Никогда не должно случиться
                };
            }
        }

        if (parser[2] is TernaryOperator tp)
        {
            res = new TernaryExpression(res, tp.FirstBranch, tp.SecondBranch);
        }

        return res;
    }
}