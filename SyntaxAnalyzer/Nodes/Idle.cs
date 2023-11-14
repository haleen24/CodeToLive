using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;


// Узел-пустышка. Используется, когда узел, полученный в результате парсинга, не нужен
// В конечном ast быть не должно
public class Idle : INode
{
    public static INode Construct(IParser parser)
    {
        return new Idle();
    }

    public IEnumerable<INode> Walk()
    {
        yield break;
    }
}