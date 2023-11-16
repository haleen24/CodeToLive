using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class IdentifierWithFinal : INode // IdentifierWithFinal
{
    public string Value { get; }
    public bool IsFinal { get; }

    public IdentifierWithFinal(string value, bool isFinal)
    {
        Value = value;
        IsFinal = isFinal;
    }

    public override string ToString()
    {
        return $"IdentifierWithFinal(name={Value})";
    }

    public IEnumerable<INode> Walk()
    {
        yield break;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new IdentifierWithFinal((parser[1] as Identifier)!.Value, parser[0] is not Idle);
    }
}