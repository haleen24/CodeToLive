using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class VariableDeclaration : INode  // IdentifierWithFinal
{
    public INode Identifier { get; }
    public bool IsFinal { get; }

    public VariableDeclaration(INode identifier, bool isFinal)
    {
        Identifier = identifier;
        IsFinal = isFinal;
    }

    public IEnumerable<INode> Walk()
    {
        yield return Identifier;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}