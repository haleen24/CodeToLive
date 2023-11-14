using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class IndexatorOperator : INode  // В конечном дереве быть не должно
{
    public IEnumerable<INode> Walk()
    {
        yield break;
    }
}