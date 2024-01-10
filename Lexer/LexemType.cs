// ReSharper disable once CheckNamespace

namespace LexerSpace
{
    /// <summary>
    /// Перечисление всех типов лексем
    /// </summary>
    public enum LexemType
    {
        Plus,
        Minus,
        Star,
        TrueDiv,
        Div,
        Mod,

        Lparenthese,
        Rparenthese,
        Lbrace,
        Rbrace,
        Lbracket,
        Rbracket,

        Band,
        Bor,
        Binv,
        Bxor,
        BLshift,
        BRshift,

        Comma,
        Dot,
        Semicolon,
        Ternary,
        Colon,

        Lambda,

        If,
        While,
        For,
        Func,
        Class,
        Interface,
        Foreach,
        Else,
        Try,
        Catch,
        Finally,
        Is,
        Break,
        Return,
        Continue,
        Import,
        Throw,
        Final,
        Field,
        Static,
        Operator,
        Conversion,
        New,
        Params,

        Assignment,

        Eqv,
        Greater,
        Less,
        GreaterEq,
        LessEq,
        NotEqv,

        True,
        False,
        Null,
        This,
        Base,
        Inner,

        And,
        Or,
        Not,

        StringLiteral,
        IntLiteral,
        FloatLiteral,
        Identifier,

        NewLine,
        LineConcat,

        PlusAssign,
        MinusAssign,
        MulAssign,
        DivAssign,
        TrueDivAssign,
        ModAssign,
        BandAssign,
        BorAssign,
        BxorAssign,
        BLshiftAssign,
        BRshiftAssign,
        AndAssign,
        OrAssign,
        
        Getter,
        Setter,
        Computable
    }
}