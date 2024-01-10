using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FormalArguments : INode
{
    public IReadOnlyList<INode> Arguments { get; }

    public FormalArguments(IEnumerable<INode> arguments)
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

    public static INode NamedArgumentsConstruct(IParser parser)
    {
        return new FormalArguments(ExtractEven(parser));
    }
    

    public static INode Construct(IParser parser)
    {
        bool allowPositional = true;
        bool allowParams = true;

        List<INode> args = new();
        foreach (INode node in ExtractEven(parser))
        {
            switch (node)
            {
                case Identifier i:
                    if (!allowPositional)
                    {
                        throw new Exception("Syntax error");  // TODO: Exceptions
                    }
                    args.Add(i);
                    break;
                case NamedArgument na:
                    allowPositional = false;
                    args.Add(na);
                    break;
                case ParamsArgument pa:
                    if (!allowParams)
                    {
                        throw new Exception("Syntax error");  // TODO: Exceptions
                    }

                    allowParams = false;
                    allowPositional = true;
                    args.Add(pa);
                    break;
                default:
                    throw new Exception("Wrong argument type");  // Никогда не должно произойти
            }
        }

        return new FormalArguments(args.AsReadOnly());
    }
}