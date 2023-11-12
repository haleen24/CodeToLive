using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class StatementSequence : INode
{
    public IReadOnlyList<INode> Statements { get; }

    public StatementSequence(IEnumerable<INode> statements)
    {
        Statements = new List<INode>(statements).AsReadOnly();
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

    public static INode Construct(IParser parser)
    {
        return new StatementSequence(GetStatements(parser));
    }
}

public class Block : StatementSequence
{
    public Block(IEnumerable<INode> statements) : base(statements)
    {
    }

    public new static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 4);  // Внутри только {, \n, StatementSequence, }
        return new Block((parser[2] as StatementSequence)!.Statements);
    }
}

public class Module : StatementSequence
{
    public Module(IEnumerable<INode> statements) : base(statements)
    {
    }
    
    public new static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 1);  // Внутри только StatementSequence
        return new Module((parser[0] as StatementSequence)!.Statements);
    }
}