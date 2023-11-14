using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;

namespace SyntaxAnalyzer.Rules;

public static class RulesMap
{
    // В этом словаре каждой грамматической единице сопоставляется праило (см. rules.txt)
    private static Dictionary<GrammarUnitType, Rule> RulesDict =>
        new Dictionary<GrammarUnitType, Rule>()
        {
            //TODO: конструкторы нод
            { GrammarUnitType.SNL, Rule.Optional(GU(LexemType.NewLine)) },
            { GrammarUnitType.Separator, Rule.Alternative(GU(LexemType.Semicolon, LexemType.NewLine)) },
            {
                GrammarUnitType.IndexatorOperator,
                new Rule(() => new Sequence(GU(LexemType.Lbracket), GU(LexemType.Rbrace)), Idle.Construct)
            },
            {
                GrammarUnitType.OverloadableOperator,
                Rule.Alternative(GU(LexemType.PlusAssign, LexemType.MinusAssign, LexemType.MulAssign,
                    LexemType.TrueDivAssign, LexemType.DivAssign, LexemType.ModAssign, LexemType.AndAssign,
                    LexemType.OrAssign, LexemType.BxorAssign, LexemType.BLshiftAssign, LexemType.BRshiftAssign,
                    LexemType.Plus, LexemType.Minus, LexemType.Product, LexemType.Div, LexemType.TrueDiv, LexemType.Mod,
                    LexemType.Band, LexemType.Bor, LexemType.Binv, LexemType.Bxor, LexemType.Eqv, LexemType.NotEqv,
                    LexemType.GreaterEq, LexemType.LessEq, LexemType.BRshift, LexemType.BLshift, LexemType.Greater,
                    LexemType.Less, GrammarUnitType.IndexatorOperator)
                )
            },

            {
                GrammarUnitType.OperatorOverload,
                new Rule(() => new Sequence(GU(LexemType.Operator, GrammarUnitType.OverloadableOperator)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.Conversion,
                new Rule(() => new Sequence(GU(LexemType.Conversion, LexemType.Lbracket, LexemType.Identifier,
                    LexemType.Rbracket)), Idle.Construct)
            },

            {
                GrammarUnitType.AttributeName,
                Rule.Alternative(GU(LexemType.Identifier, GrammarUnitType.OperatorOverload, GrammarUnitType.Conversion,
                    LexemType.New))
            },

            {
                GrammarUnitType.AttributeNameOrAccessor,
                Rule.Alternative(GU(GrammarUnitType.AttributeName, LexemType.Getter, LexemType.Setter, LexemType.Inner))
            },

            {
                GrammarUnitType.Attribute,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Dot,
                        GrammarUnitType.AttributeNameOrAccessor)), Idle.Construct)
            },

            {
                GrammarUnitType.Indexator,
                new Rule(() => new Sequence(GU(GrammarUnitType.Expression, LexemType.Lbracket, GrammarUnitType.SNL,
                    GrammarUnitType.Expression, GrammarUnitType.SNL, LexemType.Rbracket)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalFinal,
                Rule.Optional(GU(LexemType.Final))
            },

            {
                GrammarUnitType.IdentifierWithFinal,
                new Rule(() => new Sequence(GU(GrammarUnitType.OptionalFinal, LexemType.Identifier)), Idle.Construct)
            },

            {
                GrammarUnitType.Assignable,
                Rule.Alternative(GU(GrammarUnitType.Attribute, GrammarUnitType.Indexator,
                    GrammarUnitType.IdentifierWithFinal))
            },

            {
                GrammarUnitType.AssignableSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.Assignable), GU(LexemType.Comma)), Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalAssignables,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.AssignableSequence)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalAssignables,
                Rule.Optional(GU(GrammarUnitType.AdditionalAssignables))
            },

            {
                GrammarUnitType.AssignOperator,
                Rule.Alternative(GU(LexemType.Eqv, LexemType.PlusAssign, LexemType.MinusAssign, LexemType.DivAssign,
                    LexemType.MulAssign, LexemType.TrueDivAssign, LexemType.ModAssign, LexemType.BandAssign,
                    LexemType.OrAssign, LexemType.BxorAssign, LexemType.BLshiftAssign, LexemType.BRshiftAssign,
                    LexemType.AndAssign, LexemType.BorAssign)
                )
            },

            {
                GrammarUnitType.OptionalComma,
                Rule.Optional(GU(LexemType.Comma))
            },

            {
                GrammarUnitType.AssignStatement,
                new Rule(() => new Sequence(GU(GrammarUnitType.Assignable,
                    GrammarUnitType.OptionalAdditionalAssignables, GrammarUnitType.OptionalComma,
                    GrammarUnitType.AssignOperator, GrammarUnitType.Expression)), Idle.Construct)
            },

            {
                GrammarUnitType.Parenth,
                new Rule(
                    () => new Sequence(GU(LexemType.Lparenthese, GrammarUnitType.Expression, GrammarUnitType.SNL,
                        LexemType.Rparenthese)), Identical) // Уже поставил
            },

            {
                GrammarUnitType.ElseStmt,
                new Rule(() => new Sequence(GU(LexemType.Else, GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalElse,
                Rule.Optional(GU(GrammarUnitType.ElseStmt))
            },

            {
                GrammarUnitType.IfStmt,
                new Rule(() => new Sequence(GU(LexemType.If, GrammarUnitType.SNL, GrammarUnitType.Parenth,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt, GrammarUnitType.OptionalElse)), Idle.Construct)
            },

            {
                GrammarUnitType.WhileStmt,
                new Rule(() => new Sequence(GU(LexemType.While, GrammarUnitType.SNL, GrammarUnitType.Parenth,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalInlineStmt,
                Rule.Optional(GU(GrammarUnitType.InlineStmt))
            },

            {
                GrammarUnitType.OptionalExpression,
                Rule.Optional(GU(GrammarUnitType.Expression))
            },

            {
                GrammarUnitType.ForStmt,
                new Rule(() =>
                    new Sequence(GU(LexemType.For, GrammarUnitType.SNL, LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalInlineStmt, GrammarUnitType.SNL, LexemType.Semicolon,
                        GrammarUnitType.SNL, GrammarUnitType.OptionalExpression, GrammarUnitType.SNL,
                        LexemType.Semicolon, GrammarUnitType.SNL, GrammarUnitType.OptionalInlineStmt,
                        GrammarUnitType.SNL, LexemType.Rparenthese, GrammarUnitType.SNL, GrammarUnitType.Stmt
                    )), Idle.Construct)
            },

            {
                GrammarUnitType.ForeachStmt,
                new Rule(() => new Sequence(GU(LexemType.Foreach, GrammarUnitType.SNL, LexemType.Lparenthese,
                    GrammarUnitType.SNL, LexemType.Identifier, GrammarUnitType.SNL, LexemType.Colon,
                    GrammarUnitType.SNL, GrammarUnitType.Expression, GrammarUnitType.SNL, LexemType.Rparenthese,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalIdentifier,
                Rule.Optional(GU(LexemType.Identifier))
            },

            {
                GrammarUnitType.CatchStmt,
                new Rule(() => new Sequence(GU(LexemType.Catch, GrammarUnitType.SNL, LexemType.Lparenthese,
                    LexemType.Identifier, GrammarUnitType.SNL, GrammarUnitType.OptionalIdentifier, GrammarUnitType.SNL,
                    LexemType.Rparenthese, GrammarUnitType.SNL, GrammarUnitType.Stmt)), Idle.Construct)
            },

            {
                GrammarUnitType.CatchSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.CatchStmt), GU(GrammarUnitType.Separator)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.FinallyStmt,
                new Rule(() => new Sequence(GU(LexemType.Finally, GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalFinally,
                Rule.Optional(GU(GrammarUnitType.FinallyStmt))
            },

            {
                GrammarUnitType.CatchBlock,
                new Rule(() => new Sequence(GU(GrammarUnitType.CatchStmt, GrammarUnitType.Separator,
                    GrammarUnitType.CatchSequence, GrammarUnitType.Separator, GrammarUnitType.OptionalElse,
                    GrammarUnitType.Separator, GrammarUnitType.OptionalFinally)), Idle.Construct)
            },

            {
                GrammarUnitType.TryBody,
                Rule.Alternative(GU(GrammarUnitType.CatchBlock, GrammarUnitType.FinallyStmt))
            },

            {
                GrammarUnitType.TryStmt,
                new Rule(() => new Sequence(GU(LexemType.Try, GrammarUnitType.SNL, GrammarUnitType.Stmt,
                    GrammarUnitType.Separator, GrammarUnitType.TryBody)), Idle.Construct)
            },

            {
                GrammarUnitType.BreakStmt,
                new Rule(() => new Sequence(GU(LexemType.Break)), Idle.Construct)
            },

            {
                GrammarUnitType.ContinueStmt,
                new Rule(() => new Sequence(GU(LexemType.Continue)), Idle.Construct)
            },

            {
                GrammarUnitType.ReturnStmt,
                new Rule(() => new Sequence(GU(LexemType.Return, GrammarUnitType.OptionalExpression)), Idle.Construct)
            },

            {
                GrammarUnitType.ThrowStmt,
                new Rule(() => new Sequence(GU(LexemType.Throw, GrammarUnitType.Expression)), Idle.Construct)
            },

            {
                GrammarUnitType.ImportStmt,
                new Rule(() => new Sequence(GU(LexemType.Import, LexemType.StringLiteral)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalStatic,
                Rule.Optional(GU(LexemType.Static))
            },

            {
                GrammarUnitType.FieldModifier,
                Rule.Alternative(GU(LexemType.Final, LexemType.Computable))
            },

            {
                GrammarUnitType.OptionalFieldModifier,
                Rule.Optional(GU(GrammarUnitType.FieldModifier))
            },

            {
                GrammarUnitType.FieldStmt,
                new Rule(() =>
                    new Sequence(GU(GrammarUnitType.OptionalStatic, GrammarUnitType.OptionalFieldModifier,
                        LexemType.Field, LexemType.Identifier)), Idle.Construct)
            },

            {
                GrammarUnitType.NamedArgument,
                new Rule(() => new Sequence(GU(LexemType.Identifier, GrammarUnitType.SNL, LexemType.Eqv,
                    GrammarUnitType.SNL, GrammarUnitType.Expression)), Idle.Construct)
            },

            {
                GrammarUnitType.CommaWithNewLine,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL)), Idle.Construct)
            },

            {
                GrammarUnitType.PositionalFormalArguments,
                new Rule(() => new Repetition(GU(LexemType.Identifier), GU(GrammarUnitType.CommaWithNewLine)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalPositionalFormalArguments,
                new Rule(
                    () => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL,
                        GrammarUnitType.PositionalFormalArguments)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalPositionalFormalArguments,
                Rule.Optional(GU(GrammarUnitType.AdditionalPositionalFormalArguments))
            },

            {
                GrammarUnitType.NamedArguments,
                new Rule(() => new Repetition(GU(GrammarUnitType.NamedArgument), GU(GrammarUnitType.CommaWithNewLine)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalNamedArguments,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL, GrammarUnitType.NamedArguments)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalNamedArguments,
                Rule.Optional(GU(GrammarUnitType.AdditionalNamedArguments))
            },

            {
                GrammarUnitType.ParamsArgument,
                new Rule(() => new Sequence(GU(LexemType.Params, GrammarUnitType.SNL, LexemType.Identifier)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalParamsArgument,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL, GrammarUnitType.ParamsArgument)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalParamsArgument,
                Rule.Optional(GU(GrammarUnitType.AdditionalParamsArgument))
            },

            {
                GrammarUnitType.FormalArgumentsWithPositional,
                new Rule(
                    () => new Sequence(GU(LexemType.Identifier,
                        GrammarUnitType.OptionalAdditionalPositionalFormalArguments,
                        GrammarUnitType.OptionalAdditionalParamsArgument,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.FormalArgumentsWithParams,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.ParamsArgument,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.FunctionFormalArguments,
                Rule.Alternative(GU(GrammarUnitType.FormalArgumentsWithPositional,
                    GrammarUnitType.FormalArgumentsWithParams, GrammarUnitType.NamedArguments))
            },

            {
                GrammarUnitType.GetterDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Identifier, LexemType.Dot, LexemType.Getter)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.SetterDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Identifier, LexemType.Dot, LexemType.Setter)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.FunctionName,
                Rule.Alternative(GU(GrammarUnitType.AttributeName, GrammarUnitType.GetterDeclaration,
                    GrammarUnitType.SetterDeclaration, GrammarUnitType.OperatorOverload, GrammarUnitType.Conversion))
            },

            {
                GrammarUnitType.FunctionDefinition,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.OptionalStatic, LexemType.Func, GrammarUnitType.AttributeName,
                        GrammarUnitType.SNL, LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.FunctionFormalArguments, GrammarUnitType.SNL, LexemType.Rparenthese,
                        GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.SuperclassList,
                new Rule(() => new Repetition(GU(LexemType.Identifier), GU(GrammarUnitType.CommaWithNewLine)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalSuperclasses,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL, GrammarUnitType.SuperclassList)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalSuperclasses,
                Rule.Optional(GU(GrammarUnitType.AdditionalSuperclasses))
            },

            {
                GrammarUnitType.SuperclassesDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Colon, GrammarUnitType.SNL, LexemType.Identifier,
                        GrammarUnitType.SNL, GrammarUnitType.OptionalAdditionalSuperclasses)), Idle.Construct)
            },

            {
                GrammarUnitType.OptionalSuperclassesDeclaration,
                Rule.Optional(GU(GrammarUnitType.SuperclassesDeclaration))
            },

            {
                GrammarUnitType.ClassType,
                Rule.Alternative(GU(LexemType.Class, LexemType.Interface))
            },

            {
                GrammarUnitType.ClassDefinition,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.ClassType, LexemType.Identifier, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalSuperclassesDeclaration, GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalStmt,
                Rule.Optional(GU(GrammarUnitType.Stmt))
            },

            {
                GrammarUnitType.StmtSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.OptionalStmt), GU(GrammarUnitType.Separator)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.Block,
                new Rule(
                    () => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.StmtSequence, GrammarUnitType.SNL,
                        LexemType.Rbrace)), Idle.Construct)
            },

            {
                GrammarUnitType.Module,
                new Rule(() => new Sequence(GU(GrammarUnitType.StmtSequence, GrammarUnitType.SNL)), Idle.Construct)
            },

            {
                GrammarUnitType.PositionalActualArguments,
                new Rule(() => new Repetition(GU(GrammarUnitType.Expression), GU(GrammarUnitType.CommaWithNewLine)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.AdditionalPositionalActualArguments,
                new Rule(
                    () => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL,
                        GrammarUnitType.PositionalActualArguments)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalPositionalActualArguments,
                Rule.Optional(GU(GrammarUnitType.AdditionalPositionalActualArguments))
            },

            {
                GrammarUnitType.ActualArgumentsWithPositional,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression,
                        GrammarUnitType.OptionalAdditionalPositionalActualArguments,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.FunctionActualArguments,
                Rule.Alternative(GU(GrammarUnitType.ActualArgumentsWithPositional, GrammarUnitType.NamedArgument))
            },

            {
                GrammarUnitType.FunctionCall,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.FunctionActualArguments, GrammarUnitType.SNL, LexemType.Rparenthese)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.UnaryOperator,
                Rule.Alternative(GU(LexemType.Plus, LexemType.Minus, LexemType.Not, LexemType.Binv))
            },

            {
                GrammarUnitType.UnaryExpression,
                new Rule(() => new Sequence(GU(GrammarUnitType.UnaryOperator, GrammarUnitType.Expression)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.BinaryOperator,
                Rule.Alternative(GU(LexemType.Plus, LexemType.Minus, LexemType.Product, LexemType.Div,
                    LexemType.TrueDiv, LexemType.Mod, LexemType.Band, LexemType.Bor, LexemType.Bxor, LexemType.Eqv,
                    LexemType.Greater, LexemType.Less, LexemType.GreaterEq, LexemType.LessEq, LexemType.NotEqv,
                    LexemType.BLshift, LexemType.BRshift, LexemType.And, LexemType.Or, LexemType.Is))
            },

            {
                GrammarUnitType.ExpressionWithBinaryOperators,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.ExpressionWithoutBinaryOperators,
                        GrammarUnitType.BinaryOperator, GrammarUnitType.Expression)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.Expression,
                Rule.Alternative(GU(GrammarUnitType.ExpressionWithBinaryOperators,
                    GrammarUnitType.ExpressionWithoutBinaryOperators))
            },

            {
                GrammarUnitType.TernaryOperatorExpression,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Ternary, GrammarUnitType.Expression,
                        LexemType.Colon, GrammarUnitType.Expression)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.Lambda,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.FunctionFormalArguments, LexemType.Lambda,
                        GrammarUnitType.Stmt)),
                    Idle.Construct)
            },

            {
                GrammarUnitType.InlineStmt,
                Rule.Alternative(GU(GrammarUnitType.AssignStatement, GrammarUnitType.Expression))
            },

            {
                GrammarUnitType.Stmt,
                Rule.Alternative(GU(GrammarUnitType.IfStmt, GrammarUnitType.WhileStmt, GrammarUnitType.ForStmt,
                    GrammarUnitType.ForeachStmt, GrammarUnitType.FieldStmt, GrammarUnitType.TryStmt,
                    GrammarUnitType.BreakStmt, GrammarUnitType.ContinueStmt, GrammarUnitType.ReturnStmt,
                    GrammarUnitType.ThrowStmt, GrammarUnitType.ImportStmt, GrammarUnitType.FunctionDefinition,
                    GrammarUnitType.ClassDefinition, GrammarUnitType.Block, GrammarUnitType.InlineStmt))
            },

            {
                GrammarUnitType.ExpressionWithoutBinaryOperators,
                Rule.Alternative(GU(LexemType.True, LexemType.False, LexemType.Null, LexemType.This, LexemType.Base,
                    LexemType.Inner, LexemType.StringLiteral, LexemType.IntLiteral, LexemType.FloatLiteral,
                    LexemType.Identifier, GrammarUnitType.Attribute, GrammarUnitType.Indexator,
                    GrammarUnitType.Parenth, GrammarUnitType.FunctionCall, GrammarUnitType.UnaryExpression,
                    GrammarUnitType.Lambda, GrammarUnitType.TernaryOperatorExpression))
            }
        };

// Сокращения для конструкторв, чтобы меньше писать
    private static GrammarUnit GU(GrammarUnitType gut) => new GrammarUnit(gut);
    private static GrammarUnit GU(LexemType lt) => new GrammarUnit(lt);

    private static GrammarUnit[] GU(params object[] types)
    {
        LinkedList<GrammarUnit> result = new LinkedList<GrammarUnit>();
        foreach (var type in types)
        {
            switch (type)
            {
                case LexemType lexType:
                    result.AddLast(new GrammarUnit(lexType));
                    break;
                case GrammarUnitType unitType:
                    result.AddLast(new GrammarUnit(unitType));
                    break;
                default:
                    throw new Exception("Wrong Type");
            }
        }

        return result.ToArray();
    }

    private static INode Identical(IParser parser) => parser[0]; // Самый простой NodeConstructor

    public static IParser GetParser(GrammarUnit gu) // Получить описание грамматической единицы
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

    public static INode GetNode(GrammarUnit gu, IParser parser) // Получить древовидное представление
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
}