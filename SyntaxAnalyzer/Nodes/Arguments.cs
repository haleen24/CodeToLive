using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Arguments : INode // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Positional { get; }
    public INode? Params { get; }
    public IReadOnlyList<INode> Named { get; }

    public Arguments(IEnumerable<INode> positional, INode? @params, IEnumerable<INode> named)
    {
        Positional = INode.Copy(positional);
        Params = @params;
        Named = INode.Copy(named);
    }

    public IEnumerable<INode?> Walk()
    {
        foreach (INode node in Positional)
        {
            yield return node;
        }

        yield return Params;
        foreach (INode node in Named)
        {
            yield return node;
        }
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; ++i)
        {
            yield return parser[i];
        }
    }

    public static INode NamedArgumentsConstruct(IParser parser)
    {
        return new Arguments(new List<INode>(), null, Extract(parser));
    }

    public static INode AdditionalNamedArgumentsConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return parser[2];
    }

    public static INode FormalArgumentsWithPositionalConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        switch (parser[2])
        {
            case Idle:
                return new Arguments((parser[0] as PositionalArgumentsSequence)!.Arguments, parser[1], new List<INode>());
            default:
                return new Arguments((parser[0] as Arguments)!.Positional, parser[1], (parser[2] as Arguments)!.Named);
        }
    }

    public static INode FormalArgumentsWithParamsConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new Arguments(new List<INode>(), parser[1], (parser[0] as Arguments)!.Named);
    }

    public static INode ActualArgumentsWithPositionalConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new Arguments((parser[0] as Arguments)!.Positional, null, (parser[1] as Arguments)!.Named);
    }
}