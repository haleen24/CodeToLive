namespace SyntaxAnalyzer.Nodes;

public class Break : INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}