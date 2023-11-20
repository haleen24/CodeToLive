using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDeclaration : INode
{
    public INode Name { get; }
    public INode Arguments { get; }

    public FunctionDeclaration(INode name, INode arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        yield return Arguments;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 8);

        return new FunctionDeclaration(parser[1], parser[5] switch
        {
            Arguments a => a,
            Idle => new Arguments(new List<INode>(), null, new List<INode>()),
            _ => throw new Exception("Wrong type")  // Никогда не должно произойти
        });
    }
}