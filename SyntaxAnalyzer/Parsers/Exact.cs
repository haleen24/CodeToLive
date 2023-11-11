using Lexer;
using SyntaxAnalyzer.Nodes;
using System.Diagnostics;
using StringLiteral = SyntaxAnalyzer.Nodes.StringLiteral;
using FloatLiteral = SyntaxAnalyzer.Nodes.FloatLiteral;
using IntLiteral = SyntaxAnalyzer.Nodes.IntLiteral;
using Identifier = SyntaxAnalyzer.Nodes.Identifier;

namespace SyntaxAnalyzer.Parsers;

// Пытается считать из потока лексем лексему конкретного типа
// Сохраняет соответствующий узел (см. Nodes/LexemNodes.cs)
public class Exact : Parser
{
    private LexemType RequiredType { get; }
    
    private INode Result { get; set; }

    public Exact(LexemType requiredType)
    {
        RequiredType = requiredType;
    }

    private INode GetLexemNode(Lexem lexem) =>
        lexem switch
        {
            Lexer.StringLiteral sl => new StringLiteral(sl),
            Lexer.FloatLiteral fl => new FloatLiteral(fl),
            Lexer.IntLiteral il => new IntLiteral(il),
            Lexer.Identifier id => new Identifier(id),
            _ => throw new Exception("Эта лексема не может быть узлом ast")  // никогда не выполнится
        };

    public override bool Parse(LexemStream ls)
    {
        StartPosition = ls.Position;
        if (!ls.HasNext())
        {
            return false;
        }

        Lexem next = ls.Next();

        if (next.Type_ == RequiredType)
        {
            Success = true;
            Result = GetLexemNode(next);
            return true;
        }
        Rollback(ls);

        return false;
    }

    public override INode this[int id]
    {
        get
        {
            Debug.Assert(Success);
            Debug.Assert(id == 0);

            return Result;
        }
    }
}