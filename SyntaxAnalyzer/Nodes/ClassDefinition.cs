using System.Collections;
using System.Diagnostics;
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
        Superclasses = INode.Copy(superclasses);
        Body = body;
    }

    public override string ToString()
    {
        return $"ClassDefinition(name={Name}, superclasses=[{string.Join(", ", Superclasses)}], body={Body})";
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
        Debug.Assert(parser.Length == 6);
        return new ClassDefinition(parser[1], parser[3] as IEnumerable<INode>, parser[^1]);
    }
}