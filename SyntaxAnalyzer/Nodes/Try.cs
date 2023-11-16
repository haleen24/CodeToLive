using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Try : INode
{
    public INode Body { get; }
    public IReadOnlyList<INode> Catches { get; }  // Если Catch нет - будет пустой
    public INode? Else { get; }
    public INode? Finally { get; }

    public Try(INode body, IEnumerable<INode> catches, INode? @else, INode? @finally)
    {
        Body = body;
        Catches = new List<INode>(catches).AsReadOnly();
        Else = @else;
        Finally = @finally;
    }

    public override string ToString()
    {
        string @else = Else != null ? Else.ToString()! : "null";
        string @finally = Finally != null ? Finally.ToString()! : "null";
        return $"Try(body={Body}, catches=[{string.Join(", ", Catches)}], {@else}, {@finally})";
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
        Debug.Assert(parser.Length == 4);
        switch (parser[3])
        {
            case CatchBlock cb: return new Try(parser[2], cb.Catches, cb.Else, cb.Finally);
            case Nodes.Finally @finally: return new Try(parser[2], new List<INode>(), null, @finally.Body);
            default: throw new Exception("wrongType"); // Never gonna happend
        }
    }
}