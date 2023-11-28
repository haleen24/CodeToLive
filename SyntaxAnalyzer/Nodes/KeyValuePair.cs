using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class KeyValuePair : INode
{
    public INode Key { get; }
    public INode Value { get; }

    public KeyValuePair(INode key, INode value)
    {
        Key = key;
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Key;
        yield return Value;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new KeyValuePair(parser[0], parser[2]);
    }

    public override string ToString()
    {
        return $"KeyValuePair(key={Key}, value={Value})";
    }
}