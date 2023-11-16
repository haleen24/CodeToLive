using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Conversion : INode
{
    public INode ConvertTo { get; }

    public Conversion(INode convertTo)
    {
        ConvertTo = convertTo;
    }

    public override string ToString()
    {
        return $"Conversion(to={ConvertTo})";
    }

    public IEnumerable<INode> Walk()
    {
        yield return ConvertTo;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        return new Conversion(parser[2]);
    }
}