using System.Runtime.Serialization;

namespace Lexer.Exceptions;

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