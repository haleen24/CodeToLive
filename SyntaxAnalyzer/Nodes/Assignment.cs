using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Assignment : INode // Представляет оператор присваивания
{
    public IReadOnlyList<INode> Lhs { get; } // Левая часть (assignable) 
    public LexemType Sign { get; } // Знак (=, +=, -=, *=, ...)
    public INode Rhs { get; } // Правая часть (expression)

    public bool IsSeq { get; private set; }

    private Assignment(IEnumerable<INode> lhs, LexemType sign, INode rhs)
    {
        Lhs = INode.Copy(lhs);
        Sign = sign;
        Rhs = rhs;
    }

    public override string ToString()
    {
        return $"Assignment(lhs=[{string.Join(", ", Lhs)}], sign={Sign}, rhs={Rhs}, is_seq={IsSeq})";
    }

    public IEnumerable<INode> Walk()
    {
        foreach (INode node in Lhs)
        {
            yield return node;
        }

        yield return Rhs;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        var res = new Assignment((parser[0] as AssignableSequence)!.Assignables, (parser[2] as StaticLexemNode)!.Type_,
            parser[3]);

        res.IsSeq = res.Lhs.Count >= 2 || parser[1] is not Idle;
        return res;
    }
}