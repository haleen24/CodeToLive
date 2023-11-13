using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

// Описание вида `gu1 gu2 ...`
public class Sequence : Parser
{
    private new List<INode> Results { get; } = new List<INode>();
    private IEnumerable<GrammarUnit> Parsers { get; }

    public Sequence(params GrammarUnit[] parsers)
    {
        Parsers = parsers;
    }

    protected override int TrueLength => Results.Count;

    public override INode this[int id]
    {
        get
        {
            Debug.Assert(id >= 0);
            Debug.Assert(id < Length);
            return Results[id];
        }
    }

    public override bool Parse(LexemStream ls)
    {
        StartPosition = ls.Position;

        foreach (var grammarUnit in Parsers)
        {
            IParser parser = RulesMap.GetParser(grammarUnit);

            if (!parser.Parse(ls))
            {
                Rollback(ls);
                Results.Clear();
                return false;
            }

            INode node = RulesMap.GetNode(grammarUnit, parser);
            Results.Add(node);
        }

        Success = true;
        return true;
    }
}