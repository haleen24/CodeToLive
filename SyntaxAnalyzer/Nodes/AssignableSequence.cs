using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class
    AssignableSequence : INode // в итоговом дереве быть не должно
{
    public IReadOnlyList<INode> Assignables { get; }
    
    public bool IsFinal { get; }

    public AssignableSequence(IEnumerable<INode> assignables, bool isFinal)
    {
        IsFinal = isFinal;
        Assignables = INode.Copy(assignables);
    }

    public IEnumerable<INode> Walk()
    {
        return Assignables;
    }

    private static IEnumerable<INode> Extract(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }
    
    public static INode ConstructFromVariables(IParser parser)
    {
        return new AssignableSequence(Extract(parser), true);
    }

    public static INode ConstructFromExpressions(IParser parser)
    {
        return new AssignableSequence(Extract(parser), false);
    }
}