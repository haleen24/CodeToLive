using LexerSpace;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

// Здесь перечисленны узлы, непосредственно соответствующие лексемам 

public class StaticLexemNode : INode  // Представляет статическую лексему, в конечном ast быть не должно
{
    public LexemType Type_ { get; }

    public StaticLexemNode(LexemType type)
    {
        Type_ = type;
    }

    public override string ToString()
    {
        return $"{Type_}()";
    }

    public IEnumerable<INode> Walk()
    {
        yield break;
    }
}

public class True : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class False : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class Null : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class This : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class Base : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class Inner : EmptyNode, INode
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class Getter : EmptyNode, INode 
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class Setter : EmptyNode, INode 
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public class New : EmptyNode, INode 
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}

public abstract class DynamicLexemNode : INode  // Представляет динамическую лексему
{
    public string Value { get; }

    public override string ToString()
    {
        return $"{GetType().ToString().Split('.')[^1]}({Value})";
    }

    protected DynamicLexemNode(DynamicLexem lex)
    {
        Value = lex.Value;
    }

    public IEnumerable<INode> Walk()
    {
        yield break;
    }
}

public class StringLiteral : DynamicLexemNode
{
    public StringLiteral(LexerSpace.StringLiteral lex) : base(lex)
    {
    }
}

public class FloatLiteral : DynamicLexemNode
{
    public FloatLiteral(LexerSpace.FloatLiteral lex) : base(lex)
    {
    }
}

public class IntLiteral : DynamicLexemNode
{
    public IntLiteral(LexerSpace.IntLiteral lex) : base(lex)
    {
    }
}

public class Identifier : DynamicLexemNode
{
    public Identifier(LexerSpace.Identifier lex) : base(lex)
    {
    }
}



