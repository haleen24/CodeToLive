using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Assignment : INode  // Представляет оператор присваивания
{
    public INode Lhs { get; }  // Левая часть (assignable) 
    public LexemType Sign { get; }  // Знак (=, +=, -=, *=, ...)
    public INode Rhs { get; }  // Правая часть (expression)

    private Assignment(INode lhs, LexemType sign, INode rhs)
    {
        Lhs = lhs;
        Sign = sign;
        Rhs = rhs;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);  // lhs, sign, skipNewLine, rhs
        INode lhs = parser[0];
        LexemType sign = (parser[1] as StaticLexemNode)!.Type_;
        INode rhs = parser[3];
        return new Assignment(lhs, sign, rhs);
    }

    public override string ToString()
    {
        return $"Assignment(lhs={Lhs}, sign={Sign}, rhs={Rhs})";
    }
}