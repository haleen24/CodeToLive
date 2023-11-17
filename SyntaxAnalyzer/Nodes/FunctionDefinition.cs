﻿using System.Diagnostics;
using SyntaxAnalyzer.Parsers;

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
        PositionalArguments = INode.Copy(positionalArguments);
        ParamsArgument = paramsArgument;
        Body = body;
        NamedArguments = INode.Copy(namedArguments);
    }

    public override string ToString()
    {
        string @params = ParamsArgument != null ? $"params={ParamsArgument}" : "";
        return
            $"FunctionDefinition(name={Name}, positional_arguments=[{string.Join(", ", PositionalArguments)}], {@params}, named_arguments=[{string.Join(", ", NamedArguments)}], body={Body})";
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
        Debug.Assert(parser.Length == 11);
        var args = parser[6] as Arguments;
        return args == null
            ? new FunctionDefinition(parser[2], new List<INode>(), null, new List<INode>(), parser[^1])
            : new FunctionDefinition(parser[2], args!.Positional, args.Params, args.Named, parser[^1]);
    }
}