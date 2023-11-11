using System.Diagnostics;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer.Parsers;

public class Repetition : Parser
{
    private GrammarUnit ToRepeat { get; }
    private GrammarUnit? Separator { get; }
    private List<INode> Results { get; }

    public Repetition(GrammarUnit toRepeat, GrammarUnit? separator = null)
    {
        ToRepeat = toRepeat;
        Separator = separator;
        Results = new List<INode>();
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
        while (true)
        {
            {
                IParser parser = RulesMap.RulesDict[ToRepeat].Item2();
                if (!parser.Parse(ls))
                {
                    return true;
                }

                INode node = RulesMap.RulesDict[ToRepeat].Item1(parser);
                Results.Add(node);
            }

            switch (Separator)
            {
                case { } gu:
                    IParser parser = RulesMap.RulesDict[gu].Item2();
                    if (!parser.Parse(ls))
                    {
                        return true;
                    }
                    break;
            }
        }
    }
}