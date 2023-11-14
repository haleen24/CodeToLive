namespace SyntaxAnalyzer.Nodes;

public class GetterDeclaration : INode  // Для переопределения геттера поля
{
    public INode GetterOf { get; }

    public GetterDeclaration(INode getterOf)
    {
        GetterOf = getterOf;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return GetterOf;
    }
}