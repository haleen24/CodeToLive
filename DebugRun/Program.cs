using LexerSpace;
using SyntaxAnalyzer;
using SyntaxAnalyzer.Rules;

namespace DebugRun;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = "test.upl";
        string code = "fun(1, *b, 3, *d, *e, f=6, g=7, h)";
        Lexer l = new Lexer(filename, code);
        var res = l.Lex();
        Syntaxer s = new Syntaxer(new GrammarUnit(GrammarUnitType.Module));
        var res1 = s.Parse(res);
        Console.WriteLine(res1);
    }
}