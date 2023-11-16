using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class BinaryExpression : INode
{
    public INode LeftOperand { get; }
    public LexemType Operator { get; }
    public INode RightOperand { get; }

    public BinaryExpression(INode leftOperand, LexemType @operator, INode rightOperand)
    {
        LeftOperand = leftOperand;
        Operator = @operator;
        RightOperand = rightOperand;
    }

    public override string ToString()
    {
        return $"BinaryExpression(left={LeftOperand}, operation={Operator}, right={RightOperand})";
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