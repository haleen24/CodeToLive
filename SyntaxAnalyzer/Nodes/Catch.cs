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

    public IEnumerable<INode?> Walk()
    {
        yield return ExceptionClass;
        yield return Variable;
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}