using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class UnaryExpression : INode
{
    public LexemType Operator { get; }
    public INode Operand { get; }

    public UnaryExpression(LexemType @operator, INode operand)
    {
        Operator = @operator;
        Operand = operand;
    }

    public override string ToString()
    {
        return $"UnaryExpression(operation={Operator}, operand={Operand})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Operand;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new UnaryExpression((parser[0] as StaticLexemNode)!.Type_, parser[1]);
    }
}