using Lexer = Lexer.Lexer;

global::Lexer.Lexer lexer = new();

string code = 
@"
a = 3
for (i = 8; i < n; ahah = ahah 'lol') {
    kek - cheburek - 'lol'
}
";
foreach (var lexem in lexer.Lex(code))
{
    Console.WriteLine(lexem);
}