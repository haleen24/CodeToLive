using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Foreach : INode
{
    public INode Variable { get; }
    public INode Collection { get; }
    public INode Body { get; }

    public Foreach(INode variable, INode collection, INode body)
    {
        Variable = variable;
        Collection = collection;
        Body = body;
    }

    public override string ToString()
    {
        return $"Foreach(var={Variable}, collection={Collection}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Variable;
        yield return Collection;
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}