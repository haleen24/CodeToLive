using SyntaxAnalyzer.Nodes;

namespace SyntaxAnalyzer.Parsers;

public interface IParser  // Читает поток лексем, парсит его и сохраняет получившиеся узлы дерева
{
    bool Parse(LexemStream ls);
    bool Success { get; }
    INode this[int ind] { get; }
}


public abstract class Parser : IParser
{
    public abstract bool Parse(LexemStream ls);

    public bool Success { get; protected set; } = false;
    
    protected int StartPosition { get; set; }

    protected void Rollback(LexemStream ls)
    {
        ls.Position = StartPosition;
    }
    
    public abstract INode this[int id] { get; }
}

public delegate IParser ParserFactory();
