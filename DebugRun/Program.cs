using LexerSpace;
using SyntaxAnalyzer;
using SyntaxAnalyzer.Rules;

namespace DebugRun;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = "test.upl";
        string code = "2 - 3 * 5 + 4";
        Lexer l = new Lexer(filename, code);
        var res = l.Lex();
        Syntaxer s = new Syntaxer(new GrammarUnit(GrammarUnitType.Module));
        var res1 = s.Parse(res);
        Console.WriteLine(res1);
    }
}