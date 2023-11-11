﻿using Lexer;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

// Здесь перечисленны узлы, непосредственно соответствующие лексемам 

public abstract class DynamicLexemNode : INode
{
    public string Value { get; }

    public override string ToString()
    {
        return $"{GetType()}({Value})";
    }

    protected DynamicLexemNode(DynamicLexem lex)
    {
        Value = lex.Value;
    }
}

public class StringLiteral : DynamicLexemNode
{
    public StringLiteral(Lexer.StringLiteral lex) : base(lex)
    {
    }
}

public class FloatLiteral : DynamicLexemNode
{
    public FloatLiteral(Lexer.FloatLiteral lex) : base(lex)
    {
    }
}

public class IntLiteral : DynamicLexemNode
{
    public IntLiteral(Lexer.IntLiteral lex) : base(lex)
    {
    }
}

public class Identifier : DynamicLexemNode
{
    public Identifier(Lexer.Identifier lex) : base(lex)
    {
    }
}



