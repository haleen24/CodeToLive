using System;

namespace LexerSpace
{
    public class Lexem
    {
        public LexemType LType { get; }
        public int SymNumber { get; }
        public int LineNumber { get; }

        public Lexem(LexemType lType, int lineNumber, int symNumber)
        {
            SymNumber = symNumber;
            LineNumber = lineNumber;
            LType = lType;
        }

        public override string ToString()
        {
            return LType + "()";
        }
    }

    public abstract class DynamicLexem : Lexem
    {
        public string Value { get; protected init; }

        protected DynamicLexem(LexemType lType, int lineNumber, int symNumber) : base(lType, lineNumber, symNumber)
        {
            Value = "";
        }

        public override string ToString()
        {
            return LType + $"({Value})";
        }
    }

    public class StringLiteral : DynamicLexem
    {
        public StringLiteral(string val, int lineNumber, int symNumber) : base(LexemType.StringLiteral, lineNumber,
            symNumber)
        {
            Value = val;
        }
    }

    public class IntLiteral : DynamicLexem
    {
        public IntLiteral(string val, int lineNumber, int symNumber) : base(LexemType.IntLiteral, lineNumber, symNumber)
        {
            Value = val;
        }
    }

    public class FloatLiteral : DynamicLexem
    {
        public FloatLiteral(string val, int lineNumber, int symNumber) : base(LexemType.FloatLiteral, lineNumber,
            symNumber)
        {
            Value = val;
        }
    }

    public class Identifier : DynamicLexem
    {
        public Identifier(string val, int lineNumber, int symNumber) : base(LexemType.Identifier, lineNumber, symNumber)
        {
            Value = val;
        }
    }
}