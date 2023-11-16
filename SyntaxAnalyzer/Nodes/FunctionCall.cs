using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionCall : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public IReadOnlyList<INode> NamedArguments { get; }

    public FunctionCall(INode name, IEnumerable<INode> positionalArguments, IEnumerable<INode> namedArguments,
        INode body)
    {
        Name = name;
        PositionalArguments = INode.Copy(positionalArguments);
        NamedArguments = INode.Copy(namedArguments);
    }

    public override string ToString()
    {
        return
            $"FunctionCall(name={Name}, positional_arguments=[{string.Join(", ", PositionalArguments)}], named_arguments=[{string.Join(", ", NamedArguments)}])";
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
    }

    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}