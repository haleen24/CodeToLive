using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class StatementSequence : INode  // Представляет последовательность операторов произвольного характера,
                                        // в конечном ast быть не должно 
{
    public IReadOnlyList<INode> Statements { get; }

    public StatementSequence(IEnumerable<INode> statements)
    {
        Statements = INode.Copy(statements);
    }

    protected string Typename => GetType().ToString().Split(".")[^1];

    public override string ToString()
    {
        return $"{Typename}([{string.Join(", ", Statements)}])";
    }

    private static IEnumerable<INode> GetStatements(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)  // Каждое второе - разделитель
        {
            INode node = parser[i];
            if (node is not Idle)
            {
                yield return node;
            }
        }
    }

    public IEnumerable<INode> Walk()
    {
        return Statements;
    }

    public static INode Construct(IParser parser)
    {
        return new StatementSequence(GetStatements(parser));
    }
}

public class Block : StatementSequence  // Представляет последовательность операторов, обёрнутую в { }
{
    public Block(IEnumerable<INode> statements) : base(statements)
    {
    }

    public new static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 5);  // Внутри только {, snl, StatementSequence, snl, }
        return new Block((parser[2] as StatementSequence)!.Statements);
    }
}

public class Module : StatementSequence  // Корень ast 
{
    public Module(IEnumerable<INode> statements) : base(statements)
    {
    }
    
    public new static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 2);  // Внутри только StatementSequence, snl
        return new Module((parser[0] as StatementSequence)!.Statements);
    }
}