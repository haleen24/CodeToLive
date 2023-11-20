using System.Collections;
using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ClassDefinition : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> Superclasses { get; }
    public INode Body { get; }

    public LexemType Type { get; }

    public ClassDefinition(INode name, IEnumerable<INode> superclasses, INode body, LexemType classType)
    {
        Name = name;
        Superclasses = INode.Copy(superclasses);
        Body = body;
        Type = classType;
    }

    public override string ToString()
    {
        return
            $"ClassDefinition(type={Type},name={Name}, superclasses=[{string.Join(", ", Superclasses)}], body={Body})";
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

    private static INode Construct(IParser parser, LexemType lt)
    {
        Debug.Assert(parser.Length == 6);
        return new ClassDefinition(parser[1], parser[3] switch
        {
            Superclasses sc => sc.Classes,
            Idle => new List<INode>(),
            _ => throw new Exception("Wrong type")  // Никогда не должно случиться
        }, parser[^1], lt);
    }

    public static INode ConstructClass(IParser parser)
    {
        return Construct(parser, LexemType.Class);
    }

    public static INode ConstructInterface(IParser parser)
    {
        return Construct(parser, LexemType.Interface);
    }
}