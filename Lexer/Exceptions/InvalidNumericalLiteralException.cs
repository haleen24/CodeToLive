using System.Runtime.Serialization;

namespace Lexer.Exceptions;

public class InvalidNumericalLiteralException : LexerException
{
    private static string FormatMessage(params object?[] args)
    {
        string ft = FormatFileTemplate(args[..3]);
        ft += "invalid numerical literal\n";
        string lit = $"{args[3]}";
        ft += lit + "\n";
        ft += new string(' ', lit.Length - 1);
        ft += "^";
        return ft;
    }
    
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