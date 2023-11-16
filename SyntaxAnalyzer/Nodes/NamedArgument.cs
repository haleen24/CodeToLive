using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class NamedArgument : INode
{
    public INode Name { get; }
    public INode Value { get; }

    public NamedArgument(INode name, INode value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"NamedArgument(name={Name}, value={Value})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        yield return Value;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 5);
        return new NamedArgument(parser[0], parser[4]);
    }
}