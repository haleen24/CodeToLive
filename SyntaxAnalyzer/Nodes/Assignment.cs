using System.Diagnostics;
using Lexer;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Assignment : INode
{
    public INode Lhs { get; }
    public LexemType Sign { get; }
    public INode Rhs { get; }

    private Assignment(INode lhs, LexemType sign, INode rhs)
    {
        Lhs = lhs;
        Sign = sign;
        Rhs = rhs;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        INode lhs = parser[0];  // Identifier
        LexemType sign = (parser[1] as StaticLexemNode)!.Type_;
        INode rhs = parser[3];
        return new Assignment(lhs, sign, rhs);
    }

    public override string ToString()
    {
        return $"Assignment(lhs={Lhs}, sign={Sign}, rhs={Rhs})";
    }
}