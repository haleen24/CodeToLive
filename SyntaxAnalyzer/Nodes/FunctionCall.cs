using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionCall : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public INode? ParamsArgument { get; }
    public IReadOnlyList<INode> NamedArguments { get; }

    public FunctionCall(INode name, IEnumerable<INode> positionalArguments, IEnumerable<INode> namedArguments, INode? paramsArgument = null)
    {
        Name = name;
        PositionalArguments = INode.Copy(positionalArguments);
        NamedArguments = INode.Copy(namedArguments);
        ParamsArgument = paramsArgument;
    }

    public override string ToString()
    {
        string p = ParamsArgument == null ? "" : $", params_argument={ParamsArgument}";
        return
            $"FunctionCall(name={Name}, positional_arguments=[{string.Join(", ", PositionalArguments)}]{p}, named_arguments=[{string.Join(", ", NamedArguments)}])";
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
}