using LexerSpace;

Lexer lex = new Lexer("../../../../LexerTest/TestFiles/test_2.txt");
Console.WriteLine(String.Join('\n',lex.Lex()));
