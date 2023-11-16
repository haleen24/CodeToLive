using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Catch : INode
{
    public INode ExceptionClass { get; }
    public INode? Variable { get; }
    public INode Body { get; }
    
    public Catch(INode exceptionClass, INode body, INode? variable = null)
    {
        ExceptionClass = exceptionClass;
        Variable = variable;
        Body = body;
    }

    public override string ToString()
    {
        string variable = Variable == null ? $"variable={Variable}" : "";
        return $"Catch(Exception={ExceptionClass}, {variable}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return ExceptionClass;
        yield return Variable;
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        Debug.Assert(parser.Length == 10);
        return new Catch(parser[3], parser[9], parser[5]);
    }
}