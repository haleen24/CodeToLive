using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class CatchSequence : INode  // в итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Catches { get; }

    public CatchSequence(IEnumerable<INode> catches)
    {
        Catches = new List<INode>(catches).AsReadOnly();
    }

    public IEnumerable<INode?> Walk()
    {
        return Catches;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}