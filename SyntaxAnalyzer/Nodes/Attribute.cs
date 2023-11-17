using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Attribute : INode
{
    public INode AttributeOf { get; }
    public INode AttributeName { get; }

    public Attribute(INode attributeOf, INode attributeName)
    {
        AttributeOf = attributeOf;
        AttributeName = attributeName;
    }

    public override string ToString()
    {
        return $"Attribute(of={AttributeOf}, name={AttributeName})";
    }

    public IEnumerable<INode> Walk()
    {
        yield return AttributeOf;
        yield return AttributeName;
    }
    
    private static IEnumerable<INode> Nodes(IParser parser)
    {
        yield return parser[0];

        if (parser[1] is AttributeNameSequence ans)
        {
            foreach (INode an in ans.AttributeNames)
            {
                yield return an;
            }
        }

        if (parser[2] is not Idle)
        {
            yield return parser[2];
        }
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        var names = Nodes(parser);
        int i = 0;

        INode? attOf = null;
        INode? attName = null;

        foreach (INode node in names)
        {
            if (i == 0)
            {
                attOf = node;
            } else if (i == 1)
            {
                attName = node;
            }
            else
            {
                attOf = new Attribute(attOf!, attName!);
                attName = node;
            }

            ++i;
        }

        return new Attribute(attOf!, attName!);
    }
}