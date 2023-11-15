using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionCall : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public IReadOnlyList<INode> NamedArguments { get; }
    public INode Body { get; }

    public FunctionCall(INode name, IEnumerable<INode> positionalArguments, IEnumerable<INode> namedArguments, INode body)
    {
        Name = name;
        PositionalArguments = INode.Copy(positionalArguments);
        NamedArguments = INode.Copy(namedArguments);
        Body = body;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode node in PositionalArguments)
        {
            yield return node;
        }
        
        foreach (INode node in NamedArguments)
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