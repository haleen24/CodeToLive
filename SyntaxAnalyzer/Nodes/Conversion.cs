using SyntaxAnalyzer.Parsers;

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
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}