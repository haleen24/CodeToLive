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

    public IEnumerable<INode?> Walk()
    {
        yield return Operand;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}