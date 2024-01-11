using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;
using KeyValuePair = SyntaxAnalyzer.Nodes.KeyValuePair;
using Tuple = SyntaxAnalyzer.Nodes.Tuple;

namespace SyntaxAnalyzer.Rules;

public static class RulesMap
{
    // В этом словаре каждой грамматической единице сопоставляется праило (см. rules.txt)
    private static Dictionary<GrammarUnitType, Rule> RulesDict =>
        new Dictionary<GrammarUnitType, Rule>()
        {
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
                    LexemType.Plus, LexemType.Minus, LexemType.Star, LexemType.Div, LexemType.TrueDiv, LexemType.Mod,
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
                new Rule(() => new Sequence(GU(LexemType.Conversion, LexemType.Lbracket, GrammarUnitType.SNL,
                    LexemType.Identifier, GrammarUnitType.SNL,
                    LexemType.Rbracket)), Conversion.Construct)
            },

            {
                GrammarUnitType.AttributeName,
                Rule.Alternative(GU(LexemType.Identifier, GrammarUnitType.OperatorOverload, GrammarUnitType.Conversion,
                    LexemType.New))
            },

            {
                GrammarUnitType.Accessor,
                Rule.Alternative(GU(LexemType.Getter, LexemType.Setter, LexemType.New, GrammarUnitType.OperatorOverload,
                    GrammarUnitType.Conversion))
            },

            {
                GrammarUnitType.PossibleAttribute,
                Rule.Alternative(GU(GrammarUnitType.AttributeName, GrammarUnitType.Accessor))
            },

            {
                GrammarUnitType.Attribute,
                new Rule(() => new Sequence(GU(LexemType.Dot, GrammarUnitType.PossibleAttribute)),
                    AttributePart.Construct)
            },

            {
                GrammarUnitType.Indexator,
                new Rule(() => new Sequence(GU(LexemType.Lbracket, GrammarUnitType.SNL,
                        GrammarUnitType.Expression, GrammarUnitType.SNL, LexemType.Rbracket)),
                    IndexatorPart.Construct)
            },

            {
                GrammarUnitType.Variables,
                new Rule(() => new Repetition(GU(LexemType.Identifier), GU(LexemType.Comma)),
                    AssignableSequence.ConstructFromVariables)
            },

            {
                GrammarUnitType.FinalVariables,
                new Rule(() => new Sequence(GU(LexemType.Final, GrammarUnitType.Variables)), x => x[1])
            },

            {
                GrammarUnitType.LeftExpressionSequence,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.ExpressionWithoutBinaryOperators), GU(LexemType.Comma)),
                    AssignableSequence.ConstructFromExpressions)
            },

            {
                GrammarUnitType.AssignableSequence,
                Rule.Alternative(GU(GrammarUnitType.FinalVariables, GrammarUnitType.LeftExpressionSequence))
            },

            {
                GrammarUnitType.AssignOperator,
                Rule.Alternative(GU(LexemType.Assignment, LexemType.PlusAssign, LexemType.MinusAssign,
                    LexemType.DivAssign,
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
                        LexemType.Rparenthese)), x => x[1] switch
                    {
                        BinaryExpression be => be.PutInParentheses(),
                        _ => x[1]
                    }) // Уже поставил
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
                new Rule(() => new Sequence(GU(LexemType.Identifier, GrammarUnitType.SNL, LexemType.Assignment,
                    GrammarUnitType.SNL, GrammarUnitType.Expression)), NamedArgument.Construct)
            },

            {
                GrammarUnitType.CommaWithNewLine,
                new Rule(() => new Sequence(GU(LexemType.Comma, GrammarUnitType.SNL)), Comma.Construct)
            },

            {
                GrammarUnitType.ParamsFormalArgument,
                new Rule(() => new Sequence(GU(LexemType.Star, GrammarUnitType.SNL, LexemType.Identifier)),
                    ParamsArgument.Construct)
            },

            {
                GrammarUnitType.FormalArgument,
                Rule.Alternative(GU(GrammarUnitType.ParamsFormalArgument, GrammarUnitType.NamedArgument,
                    LexemType.Identifier))
            },

            {
                GrammarUnitType.FunctionFormalArguments,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.FormalArgument), GU(GrammarUnitType.CommaWithNewLine)),
                    FormalArguments.Construct
                )
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
                GrammarUnitType.FunctionDeclaration,
                new Rule(() => new Sequence(GU(LexemType.Func, GrammarUnitType.AttributeName,
                        GrammarUnitType.SNL, LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalFunctionFormalArguments, GrammarUnitType.SNL, LexemType.Rparenthese)),
                    FunctionDeclaration.Construct)
            },

            {
                GrammarUnitType.FunctionName,
                Rule.Alternative(GU(GrammarUnitType.AttributeName, GrammarUnitType.GetterDeclaration,
                    GrammarUnitType.SetterDeclaration))
            },

            {
                GrammarUnitType.FunctionDefinition,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.OptionalStatic, GrammarUnitType.FunctionDeclaration,
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
                GrammarUnitType.ClassDefinition,
                new Rule(
                    () => new Sequence(GU(LexemType.Class, LexemType.Identifier, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalSuperclassesDeclaration, GrammarUnitType.SNL, GrammarUnitType.Stmt)),
                    ClassDefinition.ConstructClass)
            },

            {
                GrammarUnitType.FunctionDeclarationSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.FunctionDeclaration), GU(GrammarUnitType.Separator)),
                    p => new Block((StatementSequence.Construct(p) as StatementSequence)!
                        .Statements))
            },

            {
                GrammarUnitType.FunctionDeclarationBlock,
                new Rule(() => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.SNL,
                    GrammarUnitType.FunctionDeclarationSequence, GrammarUnitType.SNL, LexemType.Rbrace)), x => x[2])
            },

            {
                GrammarUnitType.InterfaceBody,
                Rule.Alternative(GU(GrammarUnitType.FunctionDeclaration, GrammarUnitType.FunctionDeclarationBlock))
            },

            {
                GrammarUnitType.InterfaceDefinition,
                new Rule(() => new Sequence(GU(LexemType.Interface, LexemType.Identifier, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalSuperclassesDeclaration, GrammarUnitType.SNL,
                        GrammarUnitType.InterfaceBody)),
                    ClassDefinition.ConstructInterface)
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
                    () => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.SNL, GrammarUnitType.StmtSequence,
                        GrammarUnitType.SNL,
                        LexemType.Rbrace)), Block.Construct)
            },

            {
                GrammarUnitType.Module,
                new Rule(() => new Sequence(GU(GrammarUnitType.StmtSequence, GrammarUnitType.SNL)), Module.Construct)
            },

            {
                GrammarUnitType.ParamsActualArgument,
                new Rule(
                    () => new Sequence(GU(LexemType.Star, GrammarUnitType.SNL, GrammarUnitType.Expression)),
                    ParamsArgument.Construct
                )
            },

            {
                GrammarUnitType.ActualArgument,
                Rule.Alternative(GU(GrammarUnitType.NamedArgument, GrammarUnitType.ParamsActualArgument,
                    GrammarUnitType.Expression))
            },

            {
                GrammarUnitType.FunctionActualArguments,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.ActualArgument), GU(GrammarUnitType.CommaWithNewLine)),
                    ActualArguments.Construct
                )
            },

            {
                GrammarUnitType.OptionalFunctionActualArguments,
                Rule.Optional(GU(GrammarUnitType.FunctionActualArguments))
            },

            {
                GrammarUnitType.FunctionCall,
                new Rule(
                    () => new Sequence(GU(LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalFunctionActualArguments, GrammarUnitType.SNL, LexemType.Rparenthese)),
                    x => x[2])
            },

            {
                GrammarUnitType.UnaryOperator,
                Rule.Alternative(GU(LexemType.Plus, LexemType.Minus, LexemType.Not, LexemType.Binv))
            },

            {
                GrammarUnitType.UnaryExpression,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.UnaryOperator,
                        GrammarUnitType.ExpressionWithoutBinaryOperators)),
                    UnaryExpression.Construct)
            },

            {
                GrammarUnitType.BinaryOperator,
                Rule.Alternative(GU(LexemType.Plus, LexemType.Minus, LexemType.Star, LexemType.Div,
                    LexemType.TrueDiv, LexemType.Mod, LexemType.Band, LexemType.Bor, LexemType.Bxor, LexemType.Eqv,
                    LexemType.Greater, LexemType.Less, LexemType.GreaterEq, LexemType.LessEq, LexemType.NotEqv,
                    LexemType.BLshift, LexemType.BRshift, LexemType.And, LexemType.Or, LexemType.Is))
            },

            {
                GrammarUnitType.ExpressionWithBinaryOperators,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.ExpressionWithoutBinaryOperators),
                        GU(GrammarUnitType.BinaryOperator)),
                    BinaryExpression.Construct)
            },

            {
                GrammarUnitType.Expression,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.ExpressionWithBinaryOperators,
                        GrammarUnitType.OptionalTernaryOperator)),
                    ExpressionConstructor.Construct
                )
            },

            {
                GrammarUnitType.Lambda,
                new Rule(
                    () => new Sequence(GU(LexemType.Lparenthese, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalFunctionFormalArguments, GrammarUnitType.SNL, LexemType.Rparenthese,
                        LexemType.Lambda,
                        GrammarUnitType.Stmt)),
                    LambdaExpression.Construct)
            },

            {
                GrammarUnitType.TernaryOperator,
                new Rule(
                    () => new Sequence(GU(LexemType.Ternary, GrammarUnitType.Expression, LexemType.Colon,
                        GrammarUnitType.Expression)), TernaryOperator.Construct)
            },

            {
                GrammarUnitType.OptionalTernaryOperator,
                Rule.Optional(GU(GrammarUnitType.TernaryOperator))
            },

            {
                GrammarUnitType.ExpressionPart,
                Rule.Alternative(GU(GrammarUnitType.Attribute, GrammarUnitType.IndexatorOperator,
                    GrammarUnitType.FunctionCall))
            },

            {
                GrammarUnitType.ExpressionPartSequence,
                new Rule(() => new Repetition(GU(GrammarUnitType.ExpressionPart)), ExpressionParts.Construct)
            },

            {
                GrammarUnitType.OptionalExpressionPartSequence,
                Rule.Optional(GU(GrammarUnitType.ExpressionPartSequence))
            },

            {
                GrammarUnitType.AtomicExpression,
                Rule.Alternative(GU(LexemType.True, LexemType.False, LexemType.Null, LexemType.This, LexemType.Base,
                    LexemType.Inner, LexemType.StringLiteral, LexemType.IntLiteral, LexemType.FloatLiteral,
                    LexemType.Identifier,
                    GrammarUnitType.Lambda,
                    GrammarUnitType.Parenth, GrammarUnitType.ListLiteral,
                    GrammarUnitType.SetLiteral, GrammarUnitType.TupleLiteral, GrammarUnitType.DictLiteral,
                    GrammarUnitType.UnaryExpression))
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
                    GrammarUnitType.ClassDefinition, GrammarUnitType.InterfaceDefinition, GrammarUnitType.Block,
                    GrammarUnitType.InlineStmt))
            },

            {
                GrammarUnitType.ExpressionWithoutBinaryOperators,
                new Rule(() => new Sequence(GU(GrammarUnitType.AtomicExpression,
                        GrammarUnitType.OptionalExpressionPartSequence)),
                    ExpressionConstructor.ConstructExpressionWithoutBinaryOperators)
            },

            {
                GrammarUnitType.ExpressionSequence,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.Expression), GU(GrammarUnitType.CommaWithNewLine)),
                    ExpressionSequence.Construct
                )
            },

            {
                GrammarUnitType.OptionalExpressionSequence,
                Rule.Optional(GU(GrammarUnitType.ExpressionSequence))
            },

            {
                GrammarUnitType.ListLiteral,
                new Rule(
                    () => new Sequence(GU(LexemType.Lbracket, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalExpressionSequence, GrammarUnitType.OptionalComma, GrammarUnitType.SNL,
                        LexemType.Rbracket)),
                    List.Construct
                )
            },

            {
                GrammarUnitType.SetLiteral,
                new Rule(
                    () => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.SNL, GrammarUnitType.ExpressionSequence,
                        GrammarUnitType.OptionalComma,
                        GrammarUnitType.SNL, LexemType.Rbrace)),
                    Set.Construct
                )
            },

            {
                GrammarUnitType.TupleBody,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Comma, GrammarUnitType.SNL,
                        GrammarUnitType.OptionalExpressionSequence, GrammarUnitType.OptionalComma)),
                    Tuple.ConstructBody
                )
            },

            {
                GrammarUnitType.OptionalTupleBody,
                Rule.Optional(GU(GrammarUnitType.TupleBody))
            },

            {
                GrammarUnitType.TupleLiteral,
                new Rule(
                    () => new Sequence(GU(LexemType.Lparenthese, GrammarUnitType.SNL, GrammarUnitType.OptionalTupleBody,
                        GrammarUnitType.SNL, LexemType.Rparenthese)),
                    Tuple.Construct
                )
            },

            {
                GrammarUnitType.KeyValuePair,
                new Rule(
                    () => new Sequence(GU(GrammarUnitType.Expression, LexemType.Colon, GrammarUnitType.Expression)),
                    KeyValuePair.Construct
                )
            },

            {
                GrammarUnitType.DictBody,
                new Rule(
                    () => new Repetition(GU(GrammarUnitType.KeyValuePair), GU(GrammarUnitType.CommaWithNewLine)),
                    Dict.ConstructBody
                )
            },

            {
                GrammarUnitType.OptionalDictBody,
                Rule.Optional(GU(GrammarUnitType.DictBody))
            },

            {
                GrammarUnitType.DictLiteral,
                new Rule(
                    () => new Sequence(GU(LexemType.Lbrace, GrammarUnitType.SNL, GrammarUnitType.OptionalDictBody,
                        GrammarUnitType.OptionalComma, GrammarUnitType.SNL, LexemType.Rbrace)),
                    Dict.Construct
                )
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