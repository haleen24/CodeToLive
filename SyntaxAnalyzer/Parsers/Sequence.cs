using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Sequence : Parser
{
    private List<INode> Results { get; } = new List<INode>();
    private IEnumerable<GrammarUnit> Parsers { get; }

    public Sequence(params GrammarUnit[] parsers)
    {
        Parsers = parsers;
    }
    
    public override INode this[int id]
    {
        get
        {
            Debug.Assert(Success);
            Debug.Assert(id >= 0);
            Debug.Assert(id < Results.Count);
            return Results[id];
        }
    }

    public override bool Parse(LexemStream ls)
    {
        StartPosition = ls.Position;

        foreach (GrammarUnit grammarUnit in Parsers)
        {
            IParser parser = RulesMap.RulesDict[grammarUnit].Item2();

            if (!parser.Parse(ls))
            {
                Rollback(ls);
                Results.Clear();
                Success = true;
                return false;
            }

            NodeConstructor nc = RulesMap.RulesDict[grammarUnit].Item1;
            INode node = nc(parser);
            Results.Add(node);
        }

        return true;
    }
}