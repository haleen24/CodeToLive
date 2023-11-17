using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class BinaryExpression : INode
{
    public INode LeftOperand { get; internal set; }
    public LexemType Operator { get; }
    public INode RightOperand { get; internal set; }

    public bool InParentheses { get; private set; }

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
        throw new NotImplementedException();
    }
}