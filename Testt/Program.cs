using Lexer;
using SyntaxAnalyzer;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = "testt.upl";
        string code = "a = 3\n" +
                      "b <<= 'gerhrth'\n\n\n" +
                      "{\n" +
                      "c; 2.3\n" +
                      "b \\ \n" +
                      "-= 7" +
                      "};;";

        Lexer.Lexer lx = new Lexer.Lexer(filename, code);
        IEnumerable<Lexem> input = lx.Lex();

        Syntaxer syntaxer = new Syntaxer(new GrammarUnit(GrammarUnitType.Module));
        INode ast = syntaxer.Parse(input);
        Console.WriteLine(ast);
    }
}