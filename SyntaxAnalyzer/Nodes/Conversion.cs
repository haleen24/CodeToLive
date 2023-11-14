namespace SyntaxAnalyzer.Nodes;

public class Conversion : INode
{
    public INode ConvertTo { get; }

    public Conversion(INode convertTo)
    {
        ConvertTo = convertTo;
    }

    public IEnumerable<INode> Walk()
    {
        yield return ConvertTo;
    }
}