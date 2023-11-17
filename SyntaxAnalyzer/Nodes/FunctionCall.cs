using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionCall : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public IReadOnlyList<INode> NamedArguments { get; }

    public FunctionCall(INode name, IEnumerable<INode> positionalArguments, IEnumerable<INode> namedArguments)
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
        Debug.Assert(parser.Length == 6);
        var args = parser[3] as Arguments;
        return args == null
            ? new FunctionCall(parser[0], new List<INode>(), new List<INode>())
            : new FunctionCall(parser[0], args.Positional, args.Named);
    }
}