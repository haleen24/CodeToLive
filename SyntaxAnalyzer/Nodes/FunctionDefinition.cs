using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDefinition : INode
{
    public INode Name { get; }
    public INode Arguments { get; }
    public INode Body { get; }

    public FunctionDefinition(INode name, INode arguments, INode body)
    {
        Name = name;
        Arguments = arguments;
        Body = body;
    }

    public override string ToString()
    {
        return
            $"FunctionDefinition(name={Name}, arguments={Arguments}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        yield return Arguments;
        yield return Body;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        var args = (parser[1] as FunctionDeclaration)!;
        return new FunctionDefinition(args.Name, args.Arguments, parser[3]);
    }
}