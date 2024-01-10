using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class TransitiveOperatorChain : INode
{
    internal List<INode> RealOperands { get; }
    internal List<LexemType> RealOperators { get; }

    public IReadOnlyList<INode> Operands => RealOperands.AsReadOnly();
    public IReadOnlyList<LexemType> Operators => RealOperators.AsReadOnly();

    public TransitiveOperatorChain(IEnumerable<INode> operands, IEnumerable<LexemType> operators)
    {
        RealOperands = new List<INode>(operands);
        RealOperators = new List<LexemType>(operators);
    }

    public IEnumerable<INode?> Walk()
    {
        foreach (INode node in RealOperands)
        {
            yield return node;
        }
    }

    public override string ToString()
    {
        return
            $"TransitiveOperatorChain(operands=[{String.Join(", ", RealOperands)}], operators=[{String.Join(", ", RealOperators)}])";
    }
}

public class BinaryExpression : INode
{
    public INode LeftOperand { get; internal set; }
    public LexemType Operator { get; }
    public INode RightOperand { get; internal set; }

    public bool InParentheses { get; private set; }

    private static readonly Dictionary<LexemType, int> Priorities = new()
    {
        { LexemType.Star, 0 },
        { LexemType.Div, 0 },
        { LexemType.TrueDiv, 0 },
        { LexemType.Mod, 0 },
        { LexemType.Plus, 1 },
        { LexemType.Minus, 1 },
        { LexemType.BLshift, 2 },
        { LexemType.BRshift, 2 },
        { LexemType.Band, 3 },
        { LexemType.Bxor, 4 },
        { LexemType.Bor, 5 },
        { LexemType.Eqv, 6 },
        { LexemType.NotEqv, 6 },
        { LexemType.GreaterEq, 6 },
        { LexemType.LessEq, 6 },
        { LexemType.Greater, 6 },
        { LexemType.Less, 6 },
        { LexemType.Is, 6 },
        { LexemType.And, 7 },
        { LexemType.Or, 8 },
    };

    private static readonly int
        TransitivePriority = 6; // У всех транзитивных операторов дожен быть одинаковый приоритет,
    // так как их последовательное применение преобразовывается в конъюнкцию

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
        INode res = parser[0];

        for (int i = 1; i < parser.Length; i += 2)
        {
            LexemType op = (parser[i] as StaticLexemNode)!.Type_;
            INode rhs = parser[i + 1];
            res = ManagePriorities(res, op, rhs);
        }

        return res;
    }

    private static INode
        ManagePriorities(INode lhs, LexemType sign, INode rhs) // Добавляет новый операнд в цепочку бинарных операторов
    {
        if (Priorities[sign] == TransitivePriority) // Случай транзитивного оператора
        {
            
            if (lhs is TransitiveOperatorChain toc)  // Если слева цепочка транзитивных операторов
            // Добавляем к ней правую часть
            {
                toc.RealOperands.Add(rhs);
                toc.RealOperators.Add(sign);
                return toc;
                
            }

            // Если слева выражение без бинарных операторов или бинарный оператор с приоритетом выше, чем у транзитивных операторов
            if (lhs is not BinaryExpression be || Priorities[be.Operator] < Priorities[sign])
            {
                // Создаём новую цепочку вызовов транзитивных операторов
                return new TransitiveOperatorChain(new[] { lhs, rhs }, new[] { sign });
            }

            // Иначе создаем цепочку транзитивных операторов в правом поддереве
            be.RightOperand = ManagePriorities(be.RightOperand, sign, rhs);
            return be;
        }
        // Случай ассоциативного оператора
        
        // Если слева цпеочка транзитивных вызовов, и приоритет оператора выше транзитивного
        if (lhs is TransitiveOperatorChain toc1 && Priorities[sign] < TransitivePriority)
        {
            // Добавляем новый операндк последнему операнду цепочки транзитивных вызовов
            int lastIndex = toc1.RealOperands.Count - 1;
            toc1.RealOperands[lastIndex] = ManagePriorities(toc1.RealOperands[lastIndex], sign, rhs);
            return toc1;
        }
        // Если слева выражение без бинарных операторов или выражение в скобках
        if (lhs is not BinaryExpression leftBinary || leftBinary.InParentheses)
        {
            // Применяем к левой части правую
            return new BinaryExpression(lhs, sign, rhs);
        }

        // Если у текущего оператора приоритет ниже (т.е. выполняется он позже), чем предыдущего
        if (Priorities[sign] >= Priorities[leftBinary.Operator])
        {
            // Применяем к левой части правую
            return new BinaryExpression(lhs, sign, rhs);
        }

        // Иначе ,применям к правому операнду левой части правую часть
        leftBinary.RightOperand = ManagePriorities(leftBinary.RightOperand, sign, rhs);
        return leftBinary;
    }
}