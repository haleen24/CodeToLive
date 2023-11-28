using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Dict : INode
{
    public IReadOnlyList<INode> Elements { get; }

    public Dict(IEnumerable<INode> elements)
    {
        Elements = INode.Copy(elements);
    }

    public IEnumerable<INode?> Walk()
    {
        return Elements;
    }

    public override string ToString()
    {
        return $"Dict([{string.Join(", ", Elements)}])";
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public static INode ConstructBody(IParser parser)
    {
        return new Dict(Extract(parser));
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 6);

        return parser[2] switch
        {
            Idle => new Dict(new List<INode>()),
            Dict d => d,
            _ => throw new Exception("Wrong type for dict") // никогда не должно случиться
        };
    }
}