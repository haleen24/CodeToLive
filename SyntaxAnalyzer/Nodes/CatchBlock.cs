namespace SyntaxAnalyzer.Nodes;

public class CatchBlock : INode  // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Catches { get; }
    public INode? Else { get; }
    public INode? Finally { get; }

    public CatchBlock(IEnumerable<INode> catches, INode? @else, INode? @finally)
    {
        Catches = new List<INode>(catches).AsReadOnly();
        Else = @else;
        Finally = @finally;
    }

    public IEnumerable<INode?> Walk()
    {
        foreach (INode node in Catches)
        {
            yield return node;
        }

        yield return Else;
        yield return Finally;
    }
}