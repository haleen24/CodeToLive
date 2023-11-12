using Lexer;

namespace SyntaxAnalyzer.Rules;

public class GrammarUnit
{
    public GrammarUnitType? GUType { get; }
    public LexemType? LType { get; }

    public GrammarUnit(GrammarUnitType guType)
    {
        GUType = guType;
    }

    public GrammarUnit(LexemType lType)
    {
        LType = lType;
    }

    public override string ToString()
    {
        return GUType != null ? GUType.ToString()! : LType!.ToString()!;
    }
}