namespace LexerSpace;

public class StreamFilter
{
    private StreamReader Reader { get; }
    public int SymNumber { get; private set; } = 1;
    public int LineNumber { get; private set; } = 1;

    public StreamFilter(Stream sr)
    {
        Reader = new StreamReader(sr);
        Normalize();
    }

    public bool EndOfStream => Reader.EndOfStream;

    public void Close()
    {
        Reader.Close();
    }

    ~StreamFilter()
    {
        Close();
    }

    public char Peek()
    {
        return (char)Reader.Peek();
    }

    private void TrueAdvance()
    {
        char c = Peek();
        Reader.Read();
        if (c == '\n')
        {
            SymNumber = 1;
            LineNumber += 1;
            return;
        }

        SymNumber += 1;
    }

    public void Advance()
    {
        TrueAdvance();
        Normalize();
    }


    private void Normalize()
    {
        char c = Peek();
        if (c == '#')
        {
            do
            {
                TrueAdvance();
            } while (Peek() != '\n');
        }
    }
}