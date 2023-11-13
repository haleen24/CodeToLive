using SyntaxAnalyzer.Nodes;

namespace SyntaxAnalyzer.Parsers;

public interface IParser  // Читает поток лексем, парсит его и сохраняет получившиеся узлы дерева
{
    bool Parse(LexemStream ls);
    bool Success { get; }
    INode this[int ind] { get; }
    int Length { get; }

    internal void Rollback(LexemStream ls);
}


public abstract class Parser : IParser
{
    public abstract bool Parse(LexemStream ls);

    public bool Success { get; protected set; } = false;
    
    protected int StartPosition { get; set; }

    public void Rollback(LexemStream ls)
    {
        ls.Position = StartPosition;
    }
    
    protected abstract int TrueLength { get; }

    public int Length => Success ? TrueLength : 0;
    
    public abstract INode this[int id] { get; }

    public IEnumerable<INode> Results()
    {
        for (int i = 0; i < Length; ++i)
        {
            yield return this[i];
        }
    }
}

public delegate IParser ParserFactory();
