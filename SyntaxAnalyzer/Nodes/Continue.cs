namespace SyntaxAnalyzer.Nodes;

public class Continue : INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}