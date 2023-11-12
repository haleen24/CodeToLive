using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Alternative : Parser  // Описание вида (gu1 | gu2 | ...)
{
    private IEnumerable<GrammarUnit> Parsers { get; }
    private INode Result { get; set; }

    public Alternative(params GrammarUnit[] parsers)
    {
        Parsers = parsers;
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
        StartPosition = ls.Position;

        foreach (var grammarUnit in Parsers)
        {
            IParser parser = RulesMap.GetParser(grammarUnit);

            if (parser.Parse(ls))
            {
                INode node = RulesMap.GetNode(grammarUnit, parser);
                Result = node;
                Success = true;
                return true;
            }
        }
        Rollback(ls);
        return false;
    }
}