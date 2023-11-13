using LexerSpace;

namespace SyntaxAnalyzer.Rules;

// Представляет грамматическую единицу - простую или составную
// (по сути, union GrammarUnitType и LexemType)
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