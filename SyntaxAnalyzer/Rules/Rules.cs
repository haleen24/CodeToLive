using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Rules;

public static class RulesMap
{
    // В этом словаре описано, что означает каждая грамматическая конструкция (ключ).
    // Значением яляется кортеж из двух делегатов:
    // второй возвращает парсер, который считает из потока лексем все необходимые для грамматической
    // единицы лексемы;
    // первый конструирует узел абстрактоного синтаксического дерева на основе работы парсера
    private static Dictionary<GrammarUnitType, Rule> RulesDict =>
        new Dictionary<GrammarUnitType, Rule>()
        {
            
        };

    // Сокращения для конструкторв, чтобы меньше писать
    private static GrammarUnit GU(GrammarUnitType gut) => new GrammarUnit(gut);
    private static GrammarUnit GU(LexemType lt) => new GrammarUnit(lt);

    private static INode Identical(IParser parser) => parser[0]; // Самый простой NodeConstructor

    public static IParser GetParser(GrammarUnit gu)  // Получить описание грамматической единицы
    {
        switch (gu.GUType)
        {
            case null: // Значит LType не пустой
                LexemType lt = gu.LType!.Value;
                return new Exact(lt);
            case { } ngu:
                return RulesDict[ngu].Factory();
        }
    }

    public static INode GetNode(GrammarUnit gu, IParser parser)  // Получить древовидное представление
                                                                 // грамматической единицы
    {
        switch (gu.GUType)
        {
            case null:
                return Identical(parser);
            case { } ngu:
                return RulesDict[ngu].Constructor(parser);
        }
    }

    private static (NodeConstructor, ParserFactory) Alternative(params GrammarUnit[] args) =>
        (Identical, () => new Alternative(args));
}