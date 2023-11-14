namespace SyntaxAnalyzer.Nodes;

public class CatchSequence : INode
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
}