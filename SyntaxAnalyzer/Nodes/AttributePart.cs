using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class AttributePart : INode
{
    public INode Name { get; }

    public AttributePart(INode name)
    {
        Name = name;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new AttributePart(parser[1]);
    }
}