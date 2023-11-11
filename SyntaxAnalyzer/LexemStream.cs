using Lexer;

namespace SyntaxAnalyzer;

public class LexemStream  // Вывод лексера, с сохраненной позицией
{
    public IList<Lexem> Lexems { get; }
    public int Position { get; set; }

    public LexemStream(IList<Lexem> lexems)
    {
        Lexems = lexems;
        Position = 0;
    }

    public bool HasNext()
    {
        return Position < Lexems.Count;
    }

    public Lexem Next()
    {
        return Lexems[Position++];
    }
}