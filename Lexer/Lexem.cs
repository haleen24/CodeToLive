using System;

namespace Lexer
{
    public class Lexem
    {
        public LexemType Type_;

        public Lexem(LexemType type)
        {
            Type_ = type;
        }
        
    }

    public class StringLiteral : Lexem
    {
        public string Value { get; }

        public StringLiteral(string val) : base(LexemType.StringLiteral)
        {
            Value = val;
        }
    }

    public class IntLiteral : Lexem
    {
        public string Value { get; }
        public IntLiteral(string val) : base(LexemType.IntLiteral)
        {
            Value = val;
        }
    }
    
    public class FloatLiteral : Lexem
    {
        public string Value { get; }
        public FloatLiteral(string val) : base(LexemType.FloatLiteral)
        {
            Value = val;
        }
    }
    
    public class Identifier : Lexem
    {
        public string Value { get; }
        public Identifier(string val) : base(LexemType.Identifier)
        {
            Value = val;
        }
    }
}