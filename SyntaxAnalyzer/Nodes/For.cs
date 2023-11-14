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

    public IEnumerable<INode?> Walk()
    {
        yield return Init;
        yield return Condition;
        yield return Step;
        yield return Body;
    }
}