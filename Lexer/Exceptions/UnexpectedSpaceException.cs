using System.Runtime.Serialization;

namespace LexerSpace.Exceptions;

public class UnexpectedSpaceException : LexerException
{
    public UnexpectedSpaceException()
    {
    }

    public UnexpectedSpaceException(params object[] args) : base("unexpected whitespace symbol", args)
    {
    }

    protected UnexpectedSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UnexpectedSpaceException(string? message) : base(message)
    {
    }

    public UnexpectedSpaceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}