using LexerSpace;

namespace DebugRun;

public class Program
{
    public static void Main(string[] args)
    {
        string filename = "test.upl";
        string code = "operator +()";

        Lexer lexer = new Lexer(filename, code);

        foreach (Lexem lexem in lexer.Lex())
        {
            Console.WriteLine(lexem);
        }
    }
}