using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Finally : INode // В итоговом дереве быть не должно
{
    public INode Body { get; }

    public Finally(INode body)
    {
        Body = body;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Body;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new Finally(parser[2]);
    }
}