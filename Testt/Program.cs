using Lexer = Lexer.Lexer;

global::Lexer.Lexer lexer = new();

string path = "../../../Tests/Test_1.txt";
string result = "";
using (var i = new StreamReader(File.OpenRead(path)))
{
    while (!i.EndOfStream)
    {
        result += i.ReadLine() + '\n';
    }
}

foreach (var i in lexer.Lex(result))
{
    Console.WriteLine(i);
}