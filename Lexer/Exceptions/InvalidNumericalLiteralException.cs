using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace LexerSpace.Exceptions;

/// <summary>
/// Исключение лексера. Возникает при ошибке парсинга числовых летиралов.
/// </summary>
public class InvalidNumericalLiteralException : LexerException
{
    public InvalidNumericalLiteralException()
    {
    }

    public InvalidNumericalLiteralException(params object[] args) : base("invalid numerical literal", args)
    {
    }

    protected InvalidNumericalLiteralException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidNumericalLiteralException(string? message) : base(message)
    {
    }

    public InvalidNumericalLiteralException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}