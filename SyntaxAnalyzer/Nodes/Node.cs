using System.Collections.ObjectModel;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public interface INode  // Узел синтаксического дерева
{
    IEnumerable<INode?> Walk();

    public static IReadOnlyList<INode> Copy(IEnumerable<INode> col) => col switch
    {
        ReadOnlyCollection<INode> roc => roc,
        _ => new List<INode>(col).AsReadOnly()
    };
}

public delegate INode NodeConstructor(IParser parser);  // На основе дочерних узлов, полученных при парсинге,
                                                        // конструирует родительский 

