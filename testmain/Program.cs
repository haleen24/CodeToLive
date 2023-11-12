using LexerSpace;

Lexer lex = new Lexer("../../../../LexerTest/TestFiles/test_4.txt");
Console.WriteLine(String.Join('\n',lex.Lex()));
