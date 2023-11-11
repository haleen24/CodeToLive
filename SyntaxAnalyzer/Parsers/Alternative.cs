using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Alternative : Parser
{
    private IEnumerable<GrammarUnit> Parsers { get; }
    private INode Result { get; set; }

    public Alternative(params GrammarUnit[] parsers)
    {
        Parsers = parsers;
    }
    
    public override INode this[int id]
    {
        get
        {
            Debug.Assert(Success);
            Debug.Assert(id == 0);

            return Result;
        }
    }

    public override bool Parse(LexemStream ls)
    {
        StartPosition = ls.Position;

        foreach (GrammarUnit grammarUnit in Parsers)
        {
            IParser parser = RulesMap.RulesDict[grammarUnit].Item2();

            if (parser.Parse(ls))
            {
                INode node = RulesMap.RulesDict[grammarUnit].Item1(parser);
                Result = node;
                Success = true;
                return true;
            }
        }
        Rollback(ls);
        return false;
    }
}