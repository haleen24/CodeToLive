using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Optional : Parser
{
    private GrammarUnit Parser { get; }
    private INode Result { get; set; }

    public Optional(GrammarUnit parser)
    {
        Parser = parser;
    }

    protected override int TrueLength => 1;

    public override INode this[int id]
    {
        get
        {
            Debug.Assert(0 <= id);
            Debug.Assert(id < Length);

            return Result;
        }
    }

    public override bool Parse(LexemStream ls)
    {
        IParser parser = RulesMap.GetParser(Parser);

        if (parser.Parse(ls))
        {
            INode node = RulesMap.GetNode(Parser, parser);
            Result = node;
        }
        else
        {
            Result = new Idle();
        }

        Success = true;
        return true;
    }
}