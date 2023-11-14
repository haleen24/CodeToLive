using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Assignment : INode  // Представляет оператор присваивания
{
    public IReadOnlyList<INode> Lhs { get; }  // Левая часть (assignable) 
    public LexemType Sign { get; }  // Знак (=, +=, -=, *=, ...)
    public INode Rhs { get; }  // Правая часть (expression)

    private Assignment(IEnumerable<INode> lhs, LexemType sign, INode rhs)
    {
        Lhs = new List<INode>(lhs).AsReadOnly();
        Sign = sign;
        Rhs = rhs;
    }

    public IEnumerable<INode> Walk()
    {
        foreach (INode node in Lhs)
        {
            yield return node;
        }

        yield return Rhs;
    }
}