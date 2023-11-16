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

    public static INode Construct(IParser parser)
    {
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
        return new Arguments()
    }
}