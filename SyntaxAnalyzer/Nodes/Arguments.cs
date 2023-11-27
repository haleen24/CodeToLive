using System.Collections;
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

    public override string ToString()
    {
        string @params = Params == null ? "" : $", params={Params}, ";
        return $"Arguments(positional=[{string.Join(", ", Positional)}]{@params},named=[{string.Join(", ", Named)}])";
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

    private static IEnumerable<INode> ExtractEven(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public static INode PositionalArgumentsConstruct(IParser parser)
    {
        return new Arguments(ExtractEven(parser), null, new List<INode>());
    }

    public static INode NamedArgumentsConstruct(IParser parser)
    {
        return new Arguments(new List<INode>(), null, Extract(parser));
    }

    public static INode ArgumentsWithPositionalConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        INode? @params = parser[1] switch
        {
            Idle => null,
            _ => parser[1]
        };
        switch (parser[2])
        {
            case Idle:
                return new Arguments((parser[0] as Arguments)!.Positional, @params, new List<INode>());
            default:
                return new Arguments((parser[0] as Arguments)!.Positional, @params, (parser[2] as Arguments)!.Named);
        }
    }

    public static INode ArgumentsWithParamsConstruct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);
        return new Arguments(new List<INode>(), parser[0], parser[1] switch
        {
            Arguments args => args.Named,
            Idle => new List<INode>(),
            _ => throw new Exception("Wrong params")  // Никогда не должно произойти
        });
    }
}