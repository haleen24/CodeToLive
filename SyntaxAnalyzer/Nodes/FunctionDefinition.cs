using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDefinition : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> Arguments { get; }
    public INode Body { get; }

    public FunctionDefinition(INode name, IEnumerable<INode> arguments, INode body)
    {
        Name = name;
        Arguments = INode.Copy(arguments);
        Body = body;
    }

    public override string ToString()
    {
        return
            $"FunctionDefinition(name={Name}, arguments={String.Join(", ", Arguments)}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;

        foreach (INode node in Arguments)
        {
            yield return node;
        }
        
        yield return Body;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        Debug.Assert(parser[1] is FunctionDeclaration);
        var fd = (parser[1] as FunctionDeclaration)!;
        return new FunctionDefinition(fd.Name, fd.Arguments, parser[3]);
    }
}