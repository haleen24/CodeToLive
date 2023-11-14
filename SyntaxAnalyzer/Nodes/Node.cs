using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public interface INode  // Узел синтаксического дерева
{
    IEnumerable<INode?> Walk();
}

public delegate INode NodeConstructor(IParser parser);  // На основе дочерних узлов, полученных при парсинге,
                                                        // конструирует родительский 

