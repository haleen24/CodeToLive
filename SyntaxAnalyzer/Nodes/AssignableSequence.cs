using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class
    AssignableSequence : INode // в итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Assignables { get; }

    public AssignableSequence(IEnumerable<INode> assignables)
    {
        Assignables = INode.Copy(assignables);
    }

    public IEnumerable<INode> Walk()
    {
        return Assignables;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}