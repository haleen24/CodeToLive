using Lexer = Lexer.Lexer;

global::Lexer.Lexer lexer = new();

string code = "\"freuhuierhg'\"'1213'";
foreach (var lexem in lexer.Lex(code))
{
    Console.WriteLine(lexem);
}