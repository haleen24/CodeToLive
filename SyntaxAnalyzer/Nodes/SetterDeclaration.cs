using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class SetterDeclaration : INode // Для переопределения сеттера поля
{
    public INode SetterOf { get; }

    public SetterDeclaration(INode setterOf)
    {
        SetterOf = setterOf;
    }

    public override string ToString()
    {
        return $"SetterDeclaration(of={SetterOf})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return SetterOf;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new SetterDeclaration(parser[2]);
    }
}