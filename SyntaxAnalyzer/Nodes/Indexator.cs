using System.Diagnostics;
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

    public override string ToString()
    {
        return $"Indexator(of={IndexOf}, value={IndexValue})";
    }

    public IEnumerable<INode> Walk()
    {
        yield return IndexOf;
        yield return IndexValue;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 6);
        return new Indexator(parser[0], parser[3]);
    }
}