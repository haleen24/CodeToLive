using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Indexator : INode
{
    public INode IndexOf { get; }
    public INode IndexValue { get; }

    public Indexator(INode indexOf, INode indexValue)
    {
        IndexOf = indexOf;
        IndexValue = indexValue;
    }

    public IEnumerable<INode> Walk()
    {
        yield return IndexOf;
        yield return IndexValue;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}