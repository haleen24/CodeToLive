using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;
using Attribute = System.Attribute;

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
                new Rule(() => new Sequence(GU(LexemType.Lbracket), GU(LexemType.Rbrace)), IndexatorOperator.Construct)
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
                    OperatorOverload.Construct)
            },

            {
                GrammarUnitType.Conversion,
                new Rule(() => new Sequence(GU(LexemType.Conversion, LexemType.Lbracket, LexemType.Identifier,
                    LexemType.Rbracket)), Conversion.Construct)
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
                        GrammarUnitType.AttributeNameOrAccessor)), Nodes.Attribute.Construct)
            },

            {
                GrammarUnitType.Indexator,
                new Rule(() => new Sequence(GU(GrammarUnitType.Expression, LexemType.Lbracket, GrammarUnitType.SNL,
                    GrammarUnitType.Expression, GrammarUnitType.SNL, LexemType.Rbracket)), Indexator.Construct)
            },

            {
                GrammarUnitType.OptionalFinal,
                Rule.Optional(GU(LexemType.Final))
            },

            {
                GrammarUnitType.IdentifierWithFinal,
                new Rule(() => new Sequence(GU(GrammarUnitType.OptionalFinal, LexemType.Identifier)),
                    VariableDeclaration.Construct)
            },

            {
                GrammarUnitType.Assignable,
                Rule.Alternative(GU(GrammarUnitType.Attribute, GrammarUnitType.Indexator,
                    GrammarUnitType.IdentifierWithFinal))
            },

            {
                GrammarUnitType.AssignableSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.Assignable), GU(LexemType.Comma)),
                    AssignableSequence.Construct)
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
                new Rule(() => new Sequence(GU(GrammarUnitType.AssignableSequence, GrammarUnitType.OptionalComma,
                    GrammarUnitType.AssignOperator, GrammarUnitType.Expression)), Assignment.Construct)
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
                    Else.Construct)
            },

            {
                GrammarUnitType.OptionalElse,
                Rule.Optional(GU(GrammarUnitType.ElseStmt))
            },

            {
                GrammarUnitType.IfStmt,
                new Rule(() => new Sequence(GU(LexemType.If, GrammarUnitType.SNL, GrammarUnitType.Parenth,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt, GrammarUnitType.OptionalElse)), If.Construct)
            },

            {
                GrammarUnitType.WhileStmt,
                new Rule(() => new Sequence(GU(LexemType.While, GrammarUnitType.SNL, GrammarUnitType.Parenth,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt)), While.Construct)
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
                    )), For.Construct)
            },

            {
                GrammarUnitType.ForeachStmt,
                new Rule(() => new Sequence(GU(LexemType.Foreach, GrammarUnitType.SNL, LexemType.Lparenthese,
                    GrammarUnitType.SNL, LexemType.Identifier, GrammarUnitType.SNL, LexemType.Colon,
                    GrammarUnitType.SNL, GrammarUnitType.Expression, GrammarUnitType.SNL, LexemType.Rparenthese,
                    GrammarUnitType.SNL, GrammarUnitType.Stmt)), Foreach.Construct)
            },

            {
                GrammarUnitType.OptionalIdentifier,
                Rule.Optional(GU(LexemType.Identifier))
            },

            {
                GrammarUnitType.CatchStmt,
                new Rule(() => new Sequence(GU(LexemType.Catch, GrammarUnitType.SNL, LexemType.Lparenthese,
                    LexemType.Identifier, GrammarUnitType.SNL, GrammarUnitType.OptionalIdentifier, GrammarUnitType.SNL,
                    LexemType.Rparenthese, GrammarUnitType.SNL, GrammarUnitType.Stmt)), Catch.Construct)
            },

            {
                GrammarUnitType.CatchSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.CatchStmt)),
                    CatchSequence.Construct)
            },

            {
                GrammarUnitType.FinallyStmt,
                new Rule(() => new Sequence(GU(LexemType.Finally, GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    Finally.Construct)
            },

            {
                GrammarUnitType.OptionalFinally,
                Rule.Optional(GU(GrammarUnitType.FinallyStmt))
            },

            {
                GrammarUnitType.CatchBlock,
                new Rule(() => new Sequence(GU(
                        GrammarUnitType.CatchSequence, GrammarUnitType.OptionalElse, GrammarUnitType.OptionalFinally)),
                    CatchBlock.Construct)
            },

            {
                GrammarUnitType.TryBody,
                Rule.Alternative(GU(GrammarUnitType.CatchBlock, GrammarUnitType.FinallyStmt))
            },

            {
                GrammarUnitType.TryStmt,
                new Rule(
                    () => new Sequence(GU(LexemType.Try, GrammarUnitType.SNL, GrammarUnitType.Stmt,
                        GrammarUnitType.TryBody)), Try.Construct)
            },

            {
                GrammarUnitType.BreakStmt,
                new Rule(() => new Sequence(GU(LexemType.Break)), Break.Construct)
            },

            {
                GrammarUnitType.ContinueStmt,
                new Rule(() => new Sequence(GU(LexemType.Continue)), Continue.Construct)
            },

            {
                GrammarUnitType.ReturnStmt,
                new Rule(() => new Sequence(GU(LexemType.Return, GrammarUnitType.OptionalExpression)), Return.Construct)
            },

            {
                GrammarUnitType.ThrowStmt,
                new Rule(() => new Sequence(GU(LexemType.Throw, GrammarUnitType.Expression)), Throw.Construct)
            },

            {
                GrammarUnitType.ImportStmt,
                new Rule(() => new Sequence(GU(LexemType.Import, LexemType.StringLiteral)), Import.Construct)
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
                        LexemType.Field, LexemType.Identifier)), FieldDeclaration.Construct)
            },

            {
                GrammarUnitType.NamedArgument,
                new Rule(() => new Sequence(GU(LexemType.Identifier, GrammarUnitType.SNL, LexemType.Eqv,
                    GrammarUnitType.SNL, GrammarUnitType.Expression)), NamedArgument.Construct)
            },

            {
                GrammarUnitType.CommaWithNewLine,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL)), Comma.Construct)
            },

            {
                GrammarUnitType.PositionalFormalArguments,
                new Rule(() => new Repetition(GU(LexemType.Identifier), GU(GrammarUnitType.CommaWithNewLine)),
                    PositionalArgumentsSequence.Construct)
            },
            {
                GrammarUnitType.NamedArguments,
                new Rule(() => new Repetition(GU(GrammarUnitType.NamedArgument), GU(GrammarUnitType.CommaWithNewLine)),
                    Arguments.Construct)
            },
            {
                GrammarUnitType.AdditionalNamedArguments,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL, GrammarUnitType.NamedArguments)),
                    Arguments.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalNamedArguments,
                Rule.Optional(GU(GrammarUnitType.AdditionalNamedArguments))
            },

            {
                GrammarUnitType.ParamsArgument,
                new Rule(() => new Sequence(GU(LexemType.Params, GrammarUnitType.SNL, LexemType.Identifier)),
                    ParamsArgument.Construct)
            },

            {
                GrammarUnitType.AdditionalParamsArgument,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL, GrammarUnitType.ParamsArgument)),
                    ParamsArgument.Construct)
            },

            {
                GrammarUnitType.OptionalAdditionalParamsArgument,
                Rule.Optional(GU(GrammarUnitType.AdditionalParamsArgument))
            },

            {
                GrammarUnitType.FormalArgumentsWithPositional,
                new Rule(
                    () => new Sequence(GU(
                        GrammarUnitType.PositionalFormalArguments,
                        GrammarUnitType.OptionalAdditionalParamsArgument,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Arguments.Construct)
            },

            {
                GrammarUnitType.FormalArgumentsWithParams,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.ParamsArgument,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Arguments.Construct)
            },

            {
                GrammarUnitType.FunctionFormalArguments,
                Rule.Alternative(GU(GrammarUnitType.FormalArgumentsWithPositional,
                    GrammarUnitType.FormalArgumentsWithParams, GrammarUnitType.NamedArguments))
            },
            {
                GrammarUnitType.OptionalFunctionFormalArguments,
                Rule.Optional(GU(GrammarUnitType.FunctionFormalArguments))
            },

            {
                GrammarUnitType.GetterDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Identifier, LexemType.Dot, LexemType.Getter)),
                    GetterDeclaration.Construct)
            },

            {
                GrammarUnitType.SetterDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Identifier, LexemType.Dot, LexemType.Setter)),
                    SetterDeclaration.Construct)
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
                        GrammarUnitType.OptionalFunctionFormalArguments, GrammarUnitType.SNL, LexemType.Rparenthese,
                        GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    FunctionDefinition.Construct)
            },

            {
                GrammarUnitType.SuperclassList,
                new Rule(() => new Repetition(GU(LexemType.Identifier), GU(GrammarUnitType.CommaWithNewLine)),
                    Superclasses.Construct)
            },

            {
                GrammarUnitType.SuperclassesDeclaration,
                new Rule(
                    () => new Sequence(GU(LexemType.Colon, GrammarUnitType.SNL, GrammarUnitType.SuperclassList)),
                    Superclasses.Construct)
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
                    ClassDefinition.Construct)
            },

            {
                GrammarUnitType.OptionalStmt,
                Rule.Optional(GU(GrammarUnitType.Stmt))
            },

            {
                GrammarUnitType.NonemptyStmtSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.OptionalStmt), GU(GrammarUnitType.Separator)),
                    StatementSequence.Construct)
            },
            {
                GrammarUnitType.StmtSequence,
                Rule.Optional(GU(GrammarUnitType.NonemptyStmtSequence))
            },
            {
                GrammarUnitType.Block,
                new Rule(
                    () => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.StmtSequence, GrammarUnitType.SNL,
                        LexemType.Rbrace)), Block.Construct)
            },

            {
                GrammarUnitType.Module,
                new Rule(() => new Sequence(GU(GrammarUnitType.StmtSequence, GrammarUnitType.SNL)), Module.Construct)
            },

            {
                GrammarUnitType.PositionalActualArguments,
                new Rule(() => new Repetition(GU(GrammarUnitType.Expression), GU(GrammarUnitType.CommaWithNewLine)),
                    PositionalArgumentsSequence.Construct)
            },

            {
                GrammarUnitType.ActualArgumentsWithPositional,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.PositionalActualArguments,
                        GrammarUnitType.OptionalAdditionalNamedArguments)),
                    Arguments.Construct)
            },

            {
                GrammarUnitType.FunctionActualArguments,
                Rule.Alternative(GU(GrammarUnitType.ActualArgumentsWithPositional, GrammarUnitType.NamedArgument))
            },

            {
                GrammarUnitType.OptionalFunctionActualArguments,
                Rule.Optional(GU(GrammarUnitType.FunctionActualArguments))
            },

            {
                GrammarUnitType.FunctionCall,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalFunctionActualArguments, GrammarUnitType.SNL, LexemType.Rparenthese)),
                    FunctionCall.Construct)
            },

            {
                GrammarUnitType.UnaryOperator,
                Rule.Alternative(GU(LexemType.Plus, LexemType.Minus, LexemType.Not, LexemType.Binv))
            },

            {
                GrammarUnitType.UnaryExpression,
                new Rule(() => new Sequence(GU(GrammarUnitType.UnaryOperator, GrammarUnitType.Expression)),
                    UnaryExpression.Construct)
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
                    BinaryExpression.Construct)
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
                    TernaryExpression.Construct)
            },

            {
                GrammarUnitType.Lambda,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.FunctionFormalArguments, LexemType.Lambda,
                        GrammarUnitType.Stmt)),
                    LambdaExpression.Construct)
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