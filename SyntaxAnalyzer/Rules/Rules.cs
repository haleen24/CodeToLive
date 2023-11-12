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
    public static Dictionary<GrammarUnitType, (NodeConstructor, ParserFactory)> RulesDict =>
        new Dictionary<GrammarUnitType, (NodeConstructor, ParserFactory)>()
        {
            {
                GrammarUnitType.AtomicExpression,
                Alternative(
                    GU(LexemType.Identifier),
                    GU(LexemType.StringLiteral),
                    GU(LexemType.FloatLiteral),
                    GU(LexemType.IntLiteral))
            },
            {
                GrammarUnitType.SkipNewLine,
                (Idle.Construct,
                    () => new Optional(GU(LexemType.NewLine))
                )
            },
            {
                GrammarUnitType.Expression, // Это правило прдставляет любое выражение
                // Оно будет заполняться постепенно
                // TODO: Expression

                Alternative(GU(GrammarUnitType.AtomicExpression))
            },
            {
                GrammarUnitType.Assignable, // TODO: Assignable
                // Присваивать можно не только переменным, но и
                // полям, а также индексаторам
                Alternative(GU(LexemType.Identifier))
            },
            {
                GrammarUnitType.AssignmentOperator,
                Alternative(
                    GU(LexemType.Assignment),
                    GU(LexemType.PlusAssign),
                    GU(LexemType.MinusAssign),
                    GU(LexemType.MulAssign),
                    GU(LexemType.DivAssign),
                    GU(LexemType.TrueDivAssign),
                    GU(LexemType.ModAssign),
                    GU(LexemType.BandAssign),
                    GU(LexemType.BorAssign),
                    GU(LexemType.BxorAssign),
                    GU(LexemType.BLshiftAssign),
                    GU(LexemType.BRshiftAssign),
                    GU(LexemType.AndAssign),
                    GU(LexemType.OrAssign))
            },
            {
                GrammarUnitType.AssignmentStatement,
                (Assignment.Construct,
                    () => new Sequence(
                        GU(GrammarUnitType.Assignable),
                        GU(GrammarUnitType.AssignmentOperator),
                        GU(GrammarUnitType.SkipNewLine),
                        GU(GrammarUnitType.Expression))
                )
            },
            {
                GrammarUnitType.InlineStatement, // Statement, который можно записать в одну строку
                // Такие statement, например, могут поместиться в init цикла for
                // TODO: InlineStatement
                Alternative(
                    GU(GrammarUnitType.AssignmentStatement),
                    GU(GrammarUnitType.Expression) // Выражение - тоже statement)
                )
            },
            {
                GrammarUnitType.Statement, // Любой statement
                // TODO: Statement
                Alternative(GU(GrammarUnitType.InlineStatement), GU(GrammarUnitType.Block))
            },
            {
                GrammarUnitType.OptionalStatement,
                (Identical,
                    () => new Optional(GU(GrammarUnitType.Statement))
                )
            },
            {
                GrammarUnitType.Separator, // Разделитьтель statement-ов
                Alternative(GU(LexemType.NewLine), GU(LexemType.Semicolon))
            },
            {
                GrammarUnitType.StatementSequence,
                (StatementSequence.Construct,
                    () => new Repetition(
                        GU(GrammarUnitType.OptionalStatement),
                        GU(GrammarUnitType.Separator))
                )
            },
            {
                GrammarUnitType.Block,
                (Block.Construct,
                    () => new Sequence(
                        GU(LexemType.Lbrace),
                        GU(GrammarUnitType.SkipNewLine),
                        GU(GrammarUnitType.StatementSequence),
                        GU(LexemType.Rbrace))
                )
            },
            {
                GrammarUnitType.Module,
                (Module.Construct,
                    () => new Alternative(GU(GrammarUnitType.StatementSequence))
                )
            }
        };

    // Сокращения для конструкторв, чтобы меньше писать
    private static GrammarUnit GU(GrammarUnitType gut) => new GrammarUnit(gut);
    private static GrammarUnit GU(LexemType lt) => new GrammarUnit(lt);

    private static INode Identical(IParser parser) => parser[0]; // Самый простой NodeConstructor

    public static IParser GetParser(GrammarUnit gu)
    {
        switch (gu.GUType)
        {
            case null: // Значит LType не пустой
                LexemType lt = gu.LType ?? throw new Exception("Empty GU"); // Никогда не вылетит
                return new Exact(lt);
            case { } ngu:
                return RulesDict[ngu].Item2();
        }
    }

    public static INode GetNode(GrammarUnit gu, IParser parser)
    {
        switch (gu.GUType)
        {
            case null:
                return Identical(parser);
            case { } ngu:
                return RulesDict[ngu].Item1(parser);
        }
    }

    private static (NodeConstructor, ParserFactory) Alternative(params GrammarUnit[] args) =>
        (Identical, () => new Alternative(args));
}