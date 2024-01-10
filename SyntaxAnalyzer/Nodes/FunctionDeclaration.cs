using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDeclaration : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> Arguments { get; }

    public FunctionDeclaration(INode name, IEnumerable<INode> arguments)
    {
        Name = name;
        Arguments = INode.Copy(arguments);
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode no in Arguments)
        {
            yield return no;
        }
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 8);

        return new FunctionDeclaration(parser[1], parser[5] switch
        {
            FormalArguments fa => fa.Arguments,
            Idle => new List<INode>(),
            _ => throw new Exception("Wrong formal arguments type") // Никогда не должно произойти
        });
    }

    public override string ToString()
    {
        return $"FunctionDeclaration(name={Name}, arguments={Arguments})";
    }
}