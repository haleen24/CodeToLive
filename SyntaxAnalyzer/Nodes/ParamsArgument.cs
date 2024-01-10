using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ParamsArgument : INode
{
    public INode ToUnpack { get; }

    public ParamsArgument(INode toUnpack)
    {
        ToUnpack = toUnpack;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return ToUnpack;
    }
    
    public override string ToString()
    {
        return $"ParamsArgument(to_unpack={ToUnpack})";
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 3);
        return new ParamsArgument(parser[2]);
    }
}