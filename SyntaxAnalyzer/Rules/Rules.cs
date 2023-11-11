using Lexer;
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
    public static Dictionary<GrammarUnit, (NodeConstructor, ParserFactory)> RulesDict =>
        new Dictionary<GrammarUnit, (NodeConstructor, ParserFactory)>()
        {
            {
                GrammarUnit.Identifier,
                (Identical, () => new Exact(LexemType.Identifier))
            },
            {
                GrammarUnit.FloatLiteral,
                (Identical, () => new Exact(LexemType.FloatLiteral))
            },
            {
                GrammarUnit.IntLiteral,
                (Identical, () => new Exact(LexemType.IntLiteral))
            },
            {
                GrammarUnit.StringLiteral,
                (Identical, () => new Exact(LexemType.StringLiteral))
            },
            {
                GrammarUnit.AtomicExpression,
                (Identical,
                    () => new Alternative(
                        GrammarUnit.Identifier,
                        GrammarUnit.FloatLiteral,
                        GrammarUnit.IntLiteral,
                        GrammarUnit.StringLiteral))
            }
        };

    private static INode Identical(IParser parser) => parser[0];
}