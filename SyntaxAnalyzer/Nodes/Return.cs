﻿using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class Return : INode
{
    public INode? Value { get; }

    public Return(INode value)
    {
        Value = value;
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Value;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}