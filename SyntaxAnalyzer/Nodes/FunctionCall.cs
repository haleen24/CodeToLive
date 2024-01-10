namespace SyntaxAnalyzer.Nodes;

public class FunctionCall : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> Arguments { get; }

    public FunctionCall(INode name, IEnumerable<INode> arguments)
    {
        Name = name;
        Arguments = INode.Copy(arguments);
    }

    public override string ToString()
    {
        return
            $"FunctionCall(name={Name}, arguments=[{string.Join(", ", Arguments)}])";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode node in Arguments)
        {
            yield return node;
        }
    }
}