using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class NamedArgument : INode
{
    public INode Name { get; }
    public INode Value { get; }

    public NamedArgument(INode name, INode value)
    {
        Name = name;
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        yield return Value;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}