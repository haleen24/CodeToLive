using System.Diagnostics;
using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FieldDeclaration : INode
{
    public INode Name { get; }
    public bool IsFinal { get; }
    public bool IsComputable { get; }
    public bool IsStatic { get; }

    public FieldDeclaration(INode name, bool isStatic = false, bool isFinal = false, bool isComputable = false)
    {
        Name = name;
        IsFinal = isFinal;
        IsComputable = isComputable;
        IsStatic = isStatic;
    }

    public override string ToString()
    {
        return $"FieldDeclaration(name={Name}, is_final={IsFinal}, is_computable={IsComputable}, is_static={IsStatic})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
    }

    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);
        return parser[1] switch
        {
            Idle idle => new FieldDeclaration(parser[^1], parser[0] is not Idle),
            StaticLexemNode lexemNode => lexemNode.Type_ == LexemType.Final
                ? new FieldDeclaration(parser[^1], parser[0] is not Idle, true)
                : new FieldDeclaration(parser[^1], parser[0] is not Idle, false, true),
            _ => throw new Exception("Type Error")
        };
    }
}