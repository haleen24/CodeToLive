using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Superclasses : INode
{
    public IReadOnlyList<INode> Classes { get; }

    public Superclasses(IEnumerable<INode> classes)
    {
        Classes = new List<INode>(classes).AsReadOnly();
    }

    public IEnumerable<INode?> Walk()
    {
        return Classes;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}