using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Rules;

public class Rule
{
    public ParserFactory Factory { get; }
    public NodeConstructor Constructor { get; }

    public Rule(ParserFactory factory, NodeConstructor constructor)
    {
        Factory = factory;
        Constructor = constructor;
    }
    
    private static INode Identical(IParser parser) => parser[0]; // Самый простой NodeConstructor

    public static Rule Alternative(params GrammarUnit[] args) => new Rule(() => new Alternative(args), Identical);
    public static Rule Optional(GrammarUnit gu) => new Rule(() => new Optional(gu), Identical);
    public static Rule Additional(GrammarUnit gu) => new Rule(() => new Sequence(new GrammarUnit(LexemType.Comma), new GrammarUnit(GrammarUnitType.SNL), gu), x => x[2]);
}