using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace LexerSpace.Exceptions;

/// <summary>
/// Исключение лексера. Возникает при вводе неправильного оператора.
/// </summary>
public class InvalidOperatorException : LexerException
{
    public InvalidOperatorException()
    {
    }

    public InvalidOperatorException(params object[] args) : base("invalid operator", args)
    {
    }

    protected InvalidOperatorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidOperatorException(string? message) : base(message)
    {
    }

    public InvalidOperatorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}