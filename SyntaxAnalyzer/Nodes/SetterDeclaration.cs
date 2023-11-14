namespace SyntaxAnalyzer.Nodes;

public class SetterDeclaration : INode  // Для переопределения сеттера поля
{
    public INode SetterOf { get; }

    public SetterDeclaration(INode setterOf)
    {
        SetterOf = setterOf;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return SetterOf;
    }
}