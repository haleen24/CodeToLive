// ReSharper disable once CheckNamespace

namespace LexerSpace
{
    /// <summary>
    /// Представляет лексему - "слово" в коде
    /// </summary>
    public class Lexem
    {
        /// <value>Тип лексемы</value>
        public LexemType LType { get; }
        
        /// <value>Номер символа в строке, с которого начинается лексема</value>
        public int SymNumber { get; }
        
        /// <value>Номер строки, в которой находится лексема</value>
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

    /// <summary>
    /// Представляет динамическую лексему - заранее неизвестное слово (например, название переменной или литерал)
    /// </summary>
    public abstract class DynamicLexem : Lexem
    {
        /// <value>Значение слова</value>
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

    /// <summary>
    /// Представляет строковый литерал
    /// </summary>
    public class StringLiteral : DynamicLexem
    {
        public StringLiteral(string val, int lineNumber, int symNumber) : base(LexemType.StringLiteral, lineNumber,
            symNumber)
        {
            Value = val;
        }
    }

    /// <summary>
    /// Представляет целочисленный литерал
    /// </summary>
    public class IntLiteral : DynamicLexem
    {
        public IntLiteral(string val, int lineNumber, int symNumber) : base(LexemType.IntLiteral, lineNumber, symNumber)
        {
            Value = val;
        }
    }

    /// <summary>
    /// Представляет действительно-значный литерал 
    /// </summary>
    public class FloatLiteral : DynamicLexem
    {
        public FloatLiteral(string val, int lineNumber, int symNumber) : base(LexemType.FloatLiteral, lineNumber,
            symNumber)
        {
            Value = val;
        }
    }

    /// <summary>
    /// Представляет идентификатор - название переменной, функции, класса и т.д.
    /// </summary>
    public class Identifier : DynamicLexem
    {
        public Identifier(string val, int lineNumber, int symNumber) : base(LexemType.Identifier, lineNumber, symNumber)
        {
            Value = val;
        }
    }
}