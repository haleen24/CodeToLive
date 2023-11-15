using System.Collections;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ClassDefinition : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> Superclasses { get; }
    public INode Body { get; }

    public ClassDefinition(INode name, IEnumerable<INode> superclasses, INode body)
    {
        Name = name;
        Superclasses = new List<INode>(superclasses).AsReadOnly();
        Body = body;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode node in Superclasses)
        {
            yield return node;
        }

        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}