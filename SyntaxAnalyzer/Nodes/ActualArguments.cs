using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class ActualArguments : INode
{
    public IReadOnlyList<INode> Arguments { get; }

    public ActualArguments(IEnumerable<INode> arguments)
    {
        Arguments = INode.Copy(arguments);
    }

    public IEnumerable<INode?> Walk() => Arguments;

    private static IEnumerable<INode> ExtractEven(IParser parser)
    {
        for (int i = 0; i < parser.Length; i += 2)
        {
            yield return parser[i];
        }
    }

    public static INode Construct(IParser parser)
    {
        bool allowPositional = true;

        List<INode> args = new();

        foreach (INode node in ExtractEven(parser))
        {
            switch (node)
            {
                case NamedArgument na:
                    allowPositional = false;
                    args.Add(na);
                    break;
                default:  // Expression e
                    if (!allowPositional)
                    {
                        throw new Exception("Syntax error");  // TODO: Exceptions
                    }
                    args.Add(node);
                    break;
            }
        }

        return new ActualArguments(args.AsReadOnly());
    }
}