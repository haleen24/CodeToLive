using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Repetition : Parser
{
    private GrammarUnit ToRepeat { get; }
    private GrammarUnit? Separator { get; }
    private new List<INode> Results { get; }

    public Repetition(GrammarUnit toRepeat, GrammarUnit? separator = null)
    {
        ToRepeat = toRepeat;
        Separator = separator;
        Results = new List<INode>();
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
        while (true)
        {
            {
                IParser parser = RulesMap.GetParser(ToRepeat);
                if (!parser.Parse(ls))
                {
                    Success = true;
                    return true;
                }

                INode node = RulesMap.GetNode(ToRepeat, parser);
                Results.Add(node);
            }

            switch (Separator)
            {
                case { } gu:
                    IParser parser = RulesMap.GetParser(gu);
                    if (!parser.Parse(ls))
                    {
                        Success = true;
                        return true;
                    }
                    INode node = RulesMap.GetNode(gu, parser);
                    Results.Add(node);
                    break;
            }
        }
    }
}