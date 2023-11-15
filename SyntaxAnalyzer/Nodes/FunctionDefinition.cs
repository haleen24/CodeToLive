﻿using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Nodes;

public class FunctionDefinition : INode
{
    public INode Name { get; }
    public IReadOnlyList<INode> PositionalArguments { get; }
    public INode? ParamsArgument { get; }
    public IReadOnlyList<INode> NamedArguments { get; }
    public INode Body { get; }

    public FunctionDefinition(INode name, IEnumerable<INode> positionalArguments, INode? paramsArgument,
        IEnumerable<INode> namedArguments, INode body)
    {
        Name = name;
        PositionalArguments = new List<INode>(positionalArguments).AsReadOnly();
        ParamsArgument = paramsArgument;
        Body = body;
        NamedArguments = new List<INode>(namedArguments).AsReadOnly();
    }

    public IEnumerable<INode?> Walk()
    {
        yield return Name;
        foreach (INode node in PositionalArguments)
        {
            yield return node;
        }

        yield return ParamsArgument;
        foreach (INode node in NamedArguments)
        {
            yield return node;
        }

        yield return Body;
    }
    
    public static INode Construct(IParser parser)
    {
        throw new NotImplementedException();
    }
}