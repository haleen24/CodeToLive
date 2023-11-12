using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace LexerSpace.Exceptions;

/// <summary>
/// Ошибка лексера. Возникает, если строковый литерал не закрыт.
/// </summary>
public class UnterminatedStringLiteralException : LexerException
{
    public UnterminatedStringLiteralException()
    {
    }

    public UnterminatedStringLiteralException(params object[] args) : base("unterminated string literal", args)
    {
    }

    protected UnterminatedStringLiteralException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UnterminatedStringLiteralException(string? message) : base(message)
    {
    }

    public UnterminatedStringLiteralException(string? message, Exception? innerException) : base(message,
        innerException)
    {
    }
}