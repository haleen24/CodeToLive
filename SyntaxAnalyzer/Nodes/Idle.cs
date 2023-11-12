using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;


// Узел-пустышка. Используется, когда узел, полученный в результате парсинга, не нужен
public class Idle : INode
{
    public static INode Construct(IParser parser)
    {
        return new Idle();
    }
}