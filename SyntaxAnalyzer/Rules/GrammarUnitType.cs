namespace SyntaxAnalyzer.Rules;

public enum GrammarUnitType  // Список всех составных грамматических единиц
                             // (простые представлены LexemType)
{
    AtomicExpression,  // Объект, представимый одной лексемой (возможно - излишне)
    SkipNewLine,  // Опциональный переход на новую строку
    AssignmentOperator,  // Операция присваивания (=, +=, -=, ...)
    Expression,  // Произвольное выражение
    AssignmentStatement,  // Оператор присваивания с произвольной операцией
    InlineStatement,  // Однострочный оператор
    Statement,  // Произвольный оператор
    OptionalStatement,  // Опциональный оператор
    StatementSequence,  // Последовательность операторов
    Block,  // Блок операторов { ... }
    Module,  // Весь файл
    Separator,  // Рзделитель операторв
    Assignable  // То, что может стоять слева от операци присваивания
}