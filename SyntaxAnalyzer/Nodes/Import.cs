using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Import : INode
{
    public INode Value { get; }

    public Import(INode value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"Import(module={Value})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new Import(parser[1]);
    }
}