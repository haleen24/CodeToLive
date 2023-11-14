namespace SyntaxAnalyzer.Nodes;

public class Else : INode  // В итоговом дереве быть не должно
{
    public INode Body { get; }

    public Else(INode body)
    {
        Body = body;
    }

    public IEnumerable<INode> Walk()
    {
        yield return Body;
    }
}