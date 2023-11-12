namespace SyntaxAnalyzer.Rules;

public enum GrammarUnitType  // Список всех грамматических конструкций языка
{
    AtomicExpression,
    SkipNewLine,
    AssignmentOperator,
    Expression,
    AssignmentStatement,
    InlineStatement,
    Statement,
    OptionalStatement,
    StatementSequence,
    Block,
    Module,
    Separator,
    Assignable
}