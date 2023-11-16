using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class For : INode
{
    public INode? Init { get; }
    public INode? Condition { get; }
    public INode? Step { get; }
    public INode Body { get; }

    public For(INode body, INode? init = null, INode? condition = null, INode? step = null)
    {
        Init = init;
        Condition = condition;
        Step = step;
        Body = body;
    }

    public override string ToString()
    {
        string init = (Init != null ? Init.ToString() : "null")!;
        string cond = (Condition != null ? Condition.ToString() : "null")!;
        string step = (Step != null ? Step.ToString() : "null")!;
        return $"For(init={init}, cond={cond}, step={step}, body={Body})";
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Init;
        yield return Condition;
        yield return Step;
        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}