// ReSharper disable once CheckNamespace

namespace LexerSpace;

/// <summary>
/// Обёртка вокруг потока символов, убирающая комментарии из потока
/// </summary>
public class StreamFilter
{
    private StreamReader Reader { get; }
    /// <value>Номер символа в строке, на котором остановился поток</value>
    public int SymNumber { get; private set; } = 1;
    /// <value>Номер строки, на которой остановился поток</value>
    public int LineNumber { get; private set; } = 1;

    public StreamFilter(Stream sr)
    {
        Reader = new StreamReader(sr);
        Normalize();
    }
    
    /// <value>Кончился ли поток или нет</value>
    public bool EndOfStream => Reader.EndOfStream;

    /// <summary>
    /// Закрывает поток
    /// </summary>
    public void Close()
    {
        Reader.Close();
    }

    ~StreamFilter()
    {
        Close();
    }

    /// <summary>
    /// Читает (но не извлекает) последний символ из потока
    /// </summary>
    /// <returns>Соответствующий символ</returns>
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

    /// <summary>
    /// продвигает поток на один символ вперед, пропуская комментарии
    /// </summary>
    public void Advance()
    {
        TrueAdvance();
        Normalize();
    }


    private void Normalize()  // Пропускает комментарий
    {
        char c = Peek();
        if (c == '#')
        {
            do
            {
                TrueAdvance();
            } while (!EndOfStream && Peek() != '\n');
        }
    }
}