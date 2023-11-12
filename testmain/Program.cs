using LexerSpace;

Lexer lex = new Lexer("../../../../LexerTest/TestFiles/KeywordsTest.txt");
Console.WriteLine(String.Join('\n',lex.Lex()));
