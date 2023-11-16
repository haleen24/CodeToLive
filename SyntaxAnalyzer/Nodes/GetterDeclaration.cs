using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class GetterDeclaration : INode // Для переопределения геттера поля
{
    public INode GetterOf { get; }

    public GetterDeclaration(INode getterOf)
    {
        GetterOf = getterOf;
    }

    public override string ToString()
    {
        return $"GetterDeclaration(of={GetterOf})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return GetterOf;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new GetterDeclaration(parser[0]);
    }
}