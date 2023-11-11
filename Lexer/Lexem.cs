using System;

namespace LexerSpace
{
    public class Lexem
    {
        public LexemType Type_;

        public Lexem(LexemType type)
        {
            Type_ = type;
        }

        public override string ToString()
        {
            return Type_ + "()";
        }
    }

    public abstract class DynamicLexem : Lexem
    {
        public string Value { get; protected init; }

        protected DynamicLexem(LexemType type) : base(type)
        {
            Value = "";
        }

        public override string ToString()
        {
            return Type_ + $"({Value})";
        }
    }

    public class StringLiteral : DynamicLexem
    {
        public StringLiteral(string val) : base(LexemType.StringLiteral)
        {
            Value = val;
        }
    }

    public class IntLiteral : DynamicLexem
    {
        public IntLiteral(string val) : base(LexemType.IntLiteral)
        {
            Value = val;
        }
    }

    public class FloatLiteral : DynamicLexem
    {
        public FloatLiteral(string val) : base(LexemType.FloatLiteral)
        {
            Value = val;
        }
    }

    public class Identifier : DynamicLexem
    {
        public Identifier(string val) : base(LexemType.Identifier)
        {
            Value = val;
        }
    }
}