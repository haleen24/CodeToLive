using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Superclasses : INode // В итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Classes { get; }

    public Superclasses(IEnumerable<INode> classes)
    {
        Classes = INode.Copy(classes);
    }

    public IEnumerable<INode?> Walk()
    {
        return Classes;
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public static INode Construct(IParser parser)
    {
        return new Superclasses(Extract(parser));
    }

    public static INode AdditionalConstract(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return parser[2];
    }
}