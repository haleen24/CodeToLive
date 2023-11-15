using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Try : INode
{
    public INode Body { get; }
    public IReadOnlyList<INode> Catches { get; }  // Если Catch нет - будет пустой
    public INode Else { get; }
    public INode Finally { get; }

    public Try(INode body, IEnumerable<INode> catches, INode @else, INode @finally)
    {
        Body = body;
        Catches = new List<INode>(catches).AsReadOnly();
        Else = @else;
        Finally = @finally;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Body;
        foreach (INode node in Catches)
        {
            yield return node;
        }

        yield return Else;
        yield return Finally;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}