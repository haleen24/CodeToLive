using System.Runtime.Serialization;

namespace Lexer.Exceptions;

public class LexerException : Exception
{
    protected static string FileTemplate => "{0}:{1}:{2}: ";

    protected static string FormatMessage(string message, params object[] args)
    {
        string ft = FormatFileTemplate(args);
        ft += message + "\n";
        ft += $"{args[3]}\n";
        ft += new string(' ', (int)args[2] - 1);
        ft += "^";
        return ft;
    }
    protected static string FormatFileTemplate(params object?[] args)
    {
        return String.Format(FileTemplate, args);
    }
    
    public LexerException()
    {
    }

    public LexerException(string message, params object[] args) : base(FormatMessage(message, args))
    {
    }

    protected LexerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public LexerException(string? message) : base(message)
    {
    }

    public LexerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}