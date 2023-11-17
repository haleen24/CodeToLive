using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Return : INode
{
    public INode? Value { get; }

    public Return(INode? value)
    {
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }

    public override string ToString()
    {
        string value = (Value != null ? Value.ToString() : "null")!;
        return $"Return(value={value})";
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new Return(parser[1] is Idle ? null : parser[1]);
    }
}