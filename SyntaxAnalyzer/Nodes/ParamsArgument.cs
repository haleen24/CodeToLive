using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ParamsArgument : INode
{
    public INode Name { get; }

    public ParamsArgument(INode name)
    {
        Name = name;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
    }
    
    public override string ToString()
    {
        return $"ParamsArgument(name={Name})";
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new ParamsArgument(parser[2]);
    }
}