using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class BinaryExpression : INode
{
    public INode LeftOperand { get; internal set; }
    public LexemType Operator { get; }
    public INode RightOperand { get; internal set; }

    public bool InParentheses { get; private set; }

    private static readonly Dictionary<LexemType, int> Priorities = new()
    {
        { LexemType.Product, 0 },
        { LexemType.Div, 0 },
        { LexemType.TrueDiv, 0 },
        { LexemType.Mod, 0 },
        { LexemType.Plus, 1 },
        { LexemType.Minus, 1 },
        { LexemType.Band, 2 },
        { LexemType.Bor, 2 },
        { LexemType.Bxor, 2 },
        { LexemType.BLshift, 2 },
        { LexemType.BRshift, 2 },
        { LexemType.Eqv, 3 },
        { LexemType.NotEqv, 3 },
        { LexemType.GreaterEq, 3 },
        { LexemType.LessEq, 3 },
        { LexemType.Greater, 3 },
        { LexemType.Less, 3 },
        { LexemType.Is, 3 },
        { LexemType.And, 4 },
        { LexemType.Or, 5 },
    };

    private static readonly HashSet<LexemType> InversedOperators = new HashSet<LexemType>()
    {
        LexemType.Minus,
        LexemType.Div,
        LexemType.Mod,
        LexemType.TrueDiv
    };

    private static bool IsTransitive(LexemType op) => Priorities[op] == 3;

    public BinaryExpression(INode leftOperand, LexemType @operator, INode rightOperand, bool inParentheses = false)
    {
        LeftOperand = leftOperand;
        Operator = @operator;
        RightOperand = rightOperand;
        InParentheses = inParentheses;
    }

    public BinaryExpression PutInParentheses()
    {
        InParentheses = true;
        return this;
    }

    public override string ToString()
    {
        return
            $"BinaryExpression(left={LeftOperand}, operation={Operator}, right={RightOperand}, in_parentheses={InParentheses})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return LeftOperand;
        yield return RightOperand;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return ManagePriorities(parser[0], (parser[1] as StaticLexemNode)!.Type_, parser[2]);
    }

    private static INode ManagePriorities(INode lhs, LexemType sign, INode rhs)
    {
        if (rhs is not BinaryExpression rightBinary || rightBinary.InParentheses )
        {
            return new BinaryExpression(lhs, sign, rhs);
        }
        
        if (InversedOperators.Contains(sign) && Priorities[sign] == Priorities[rightBinary.Operator])
        {
            return new BinaryExpression(
                new BinaryExpression(lhs, sign, rightBinary.LeftOperand),
                rightBinary.Operator,
                rightBinary.RightOperand
            );
        }

        if (IsTransitive(sign) && IsTransitive(rightBinary.Operator))
        {
            return new BinaryExpression(
                new BinaryExpression(lhs, sign, rightBinary.LeftOperand),
                LexemType.And,
                rightBinary
            );
        }
        
        if (Priorities[sign] >= Priorities[rightBinary.Operator])
        {
            return new BinaryExpression(lhs, sign, rhs);
        }

        rightBinary.LeftOperand = ManagePriorities(lhs, sign, rightBinary.LeftOperand);
        return rightBinary;
    }
}