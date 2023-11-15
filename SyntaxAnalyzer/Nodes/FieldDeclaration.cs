using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FieldDeclaration : INode
{
    public INode Name { get; }
    public bool IsFinal { get; }
    public bool IsComputable { get; }
    public bool IsStatic { get; }

    public FieldDeclaration(INode name, bool isFinal, bool isComputable, bool isStatic)
    {
        Name = name;
        IsFinal = isFinal;
        IsComputable = isComputable;
        IsStatic = isStatic;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}