using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class OperatorOverload
{
    public enum OverloadableOperator
    {
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
        Plus,
        Minus,
        Product,
        TrueDiv,
        Div,
        Mod,
        Band,
        Bor,
        Binv,
        Bxor,
        BLshift,
        BRshift,
        Eqv,
        Greater,
        Less,
        GreaterEq,
        LessEq,
        NotEqv,
        IndexatorOperator
    }

    private OverloadableOperator GetOperator(INode node)
    {
        switch (node)
        {
            case StaticLexemNode ln:
                return Enum.Parse<OverloadableOperator>(ln.Type_.ToString());
            case IndexatorOperator:
                return OverloadableOperator.IndexatorOperator;
            default:
                throw new Exception("Wrong node");  // must never happen 
        }
    }
    
    public OverloadableOperator Operator { get; }

    public OperatorOverload(OverloadableOperator @operator)
    {
        Operator = @operator;
    }

    public override string ToString()
    {
        return $"OperatorOverload(of={Operator})";
    }

    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}