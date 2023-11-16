using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ParamsArgument : INode // В итоговом дереве быть не должно
{
    public INode Argument { get; }

    public ParamsArgument(INode argument)
    {
        Argument = argument;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Argument;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new ParamsArgument(parser[2]);
    }

    public static INode AdditionalConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        return parser[3];
    }
}   