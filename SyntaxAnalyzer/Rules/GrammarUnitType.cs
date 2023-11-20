﻿namespace SyntaxAnalyzer.Rules;

public enum GrammarUnitType  // Список всех составных грамматических единиц
                             // (простые представлены LexemType)
                             // Определения записан в rules.txt
{
    SNL,
    Separator,
    IndexatorOperator,
    OverloadableOperator,
    OperatorOverload,
    Conversion,
    AttributeName,
    Accessor,
    Attribute,
    Indexator,
    Variables,
    FinalVariables,
    ExpressionSequence,
    AssignableSequence,
    AssignOperator,
    OptionalComma,
    AssignStatement,
    Parenth,
    ElseStmt,
    OptionalElse,
    IfStmt,
    WhileStmt,
    OptionalInlineStmt,
    OptionalExpression,
    ForStmt,
    ForeachStmt,
    OptionalIdentifier,
    CatchStmt,
    CatchSequence,
    FinallyStmt,
    OptionalFinally,
    CatchBlock,
    TryBody,
    TryStmt,
    BreakStmt,
    ContinueStmt,
    ReturnStmt,
    ThrowStmt,
    ImportStmt,
    OptionalStatic,
    FieldModifier,
    OptionalFieldModifier,
    FieldStmt,
    NamedArgument,
    CommaWithNewLine,
    PositionalFormalArguments,
    NamedArguments,
    AdditionalNamedArguments,
    OptionalAdditionalNamedArguments,
    ParamsArgument,
    AdditionalParamsArgument,
    OptionalAdditionalParamsArgument,
    FormalArgumentsWithPositional,
    FormalArgumentsWithParams,
    FunctionFormalArguments,
    OptionalFunctionFormalArguments,
    GetterDeclaration,
    SetterDeclaration,
    FunctionName,
    FunctionDefinition,
    SuperclassList,
    SuperclassesDeclaration,
    OptionalSuperclassesDeclaration,
    ClassType,
    ClassDefinition,
    OptionalStmt,
    NonemptyStmtSequence,
    StmtSequence,
    Block,
    Module,
    PositionalActualArguments,
    ActualArgumentsWithPositional,
    FunctionActualArguments,
    OptionalFunctionActualArguments,
    FunctionCall,
    UnaryOperator,
    UnaryExpression,
    BinaryOperator,
    ExpressionWithBinaryOperators,
    Expression,
    TernaryOperator,
    OptionalTernaryOperator,
    PossibleAttribute,
    ExpressionPart,
    ExpressionPartSequence,
    OptionalExpressionPartSequence,
    AtomicExpression,
    Lambda,
    InlineStmt,
    Stmt,
    ExpressionWithoutBinaryOperators,
}