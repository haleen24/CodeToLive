using LexerSpace;
using SyntaxAnalyzer.Nodes;
using System.Diagnostics;
using StringLiteral = SyntaxAnalyzer.Nodes.StringLiteral;
using FloatLiteral = SyntaxAnalyzer.Nodes.FloatLiteral;
using IntLiteral = SyntaxAnalyzer.Nodes.IntLiteral;
using Identifier = SyntaxAnalyzer.Nodes.Identifier;

namespace SyntaxAnalyzer.Parsers;

// описание вида *конкретная лексема*
public class Exact : Parser
{
    private LexemType RequiredType { get; }
    
    private INode Result { get; set; }

    public Exact(LexemType requiredType)
    {
        RequiredType = requiredType;
    }

    protected override int TrueLength => 1;

    private INode GetLexemNode(Lexem lexem) =>
        lexem switch
        {
            LexerSpace.StringLiteral sl => new StringLiteral(sl),
            LexerSpace.FloatLiteral fl => new FloatLiteral(fl),
            LexerSpace.IntLiteral il => new IntLiteral(il),
            LexerSpace.Identifier id => new Identifier(id),
            _ => lexem.LType switch
            {
                LexemType.True => new True(),
                LexemType.False => new False(),
                LexemType.Null => new Null(),
                LexemType.This => new This(),
                LexemType.Base => new Base(),
                LexemType.Inner => new Inner(),
                LexemType.Getter => new Getter(),
                LexemType.Setter => new Setter(),
                LexemType.New => new New(),
                _ => new StaticLexemNode(lexem.LType)
            }
        };

    public override bool Parse(LexemStream ls)
    {
        StartPosition = ls.Position;
        if (!ls.HasNext())
        {
            return false;
        }

        Lexem next = ls.Next();

        if (next.LType == RequiredType)
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
            Debug.Assert(0 <= id);
            Debug.Assert(id < Length);

            return Result;
        }
    }
}