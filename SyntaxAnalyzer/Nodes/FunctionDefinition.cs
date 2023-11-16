using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDefinition : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public INode? ParamsArgument { get; }
    public IReadOnlyList<INode> NamedArguments { get; }
    public INode Body { get; }

    public FunctionDefinition(INode name, IEnumerable<INode> positionalArguments, INode? paramsArgument,
        IEnumerable<INode> namedArguments, INode body)
    {
        Name = name;
        PositionalArguments = INode.Copy(positionalArguments);
        ParamsArgument = paramsArgument;
        Body = body;
        NamedArguments = INode.Copy(namedArguments);
    }

    public override string ToString()
    {
        string @params = ParamsArgument != null ? $"params={ParamsArgument}" : "";
        return
            $"FunctionDefinition(name={Name}, positional_arguments=[{string.Join(", ", PositionalArguments)}], {@params}, named_arguments=[{string.Join(", ", NamedArguments)}], body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode node in PositionalArguments)
        {
            yield return node;
        }

        yield return ParamsArgument;
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