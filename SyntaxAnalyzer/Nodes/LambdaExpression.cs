using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class LambdaExpression : INode
{
    public IReadOnlyList<INode> PositionalArguments { get; }
    public IReadOnlyList<INode> NamedArguments { get; }
    public INode Body { get; }

    public LambdaExpression(IEnumerable<INode> positionalArguments, IEnumerable<INode> namedArguments, INode body)
    {
        PositionalArguments = new List<INode>(positionalArguments).AsReadOnly();
        NamedArguments = new List<INode>(namedArguments).AsReadOnly();
        Body = body;
    }

    public IEnumerable<INode?> Walk()
    {
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