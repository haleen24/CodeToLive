using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Superclasses : INode  // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Classes { get; }

    public Superclasses(IEnumerable<INode> classes)
    {
        Classes = INode.Copy(classes);
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