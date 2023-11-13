using System.Diagnostics;
using System.Text;
using LexerSpace.Exceptions;

// ReSharper disable once CheckNamespace
namespace LexerSpace
{
    /// <summary>
    /// Класс лексера. Преобразует поток символов в поток слов - лексем.
    /// </summary>
    public class Lexer
    {
        /// <summary>
        /// Словарь ключевых слов и соответствующих типов лексем
        /// </summary>
        private static readonly Dictionary<string, LexemType> KeyWords = new Dictionary<string, LexemType>
        {
            { "if", LexemType.If },
            { "while", LexemType.While },
            { "for", LexemType.For },
            { "foreach", LexemType.Foreach },
            { "func", LexemType.Func },
            { "class", LexemType.Class },
            { "interface", LexemType.Interface },
            { "true", LexemType.True },
            { "false", LexemType.False },
            { "null", LexemType.Null },
            { "is", LexemType.Is },
            { "break", LexemType.Break },
            { "return", LexemType.Return },
            { "continue", LexemType.Continue },
            { "else", LexemType.Else },
            { "try", LexemType.Try },
            { "catch", LexemType.Catch },
            { "finally", LexemType.Finally },
            { "import", LexemType.Import },
            { "throw", LexemType.Throw },
            { "final", LexemType.Final },
            { "field", LexemType.Field },
            { "static", LexemType.Static },
            { "operator", LexemType.Operator },
            { "conversion", LexemType.Conversion },
            { "this", LexemType.This },
            { "base", LexemType.Base }
        };

        /// <summary>
        /// Словрь "прерывающих" операторов - символов, не являющихся частью каких-либо других операторов -
        /// и соответствующих типов лексем 
        /// </summary>
        private static readonly Dictionary<string, LexemType> TerminatingOperators = new Dictionary<string, LexemType>
        {
            { "(", LexemType.Lparenthese },
            { ")", LexemType.Rparenthese },
            { "{", LexemType.Lbrace },
            { "}", LexemType.Rbrace },
            { "[", LexemType.Lbracket },
            { "]", LexemType.Rbracket },
            { ",", LexemType.Comma },
            { "\\", LexemType.LineConcat },
            { ";", LexemType.Semicolon }
        };

        /// <summary>
        /// Словарь операторов и соответствующих типов лексем
        /// </summary>
        private static readonly Dictionary<string, LexemType> NonTerminatingOperators =
            new Dictionary<string, LexemType>
            {
                { "+", LexemType.Plus },
                { "-", LexemType.Minus },
                { "*", LexemType.Product },
                { "/", LexemType.TrueDiv },
                { "//", LexemType.Div },
                { "%", LexemType.Mod },
                { "!", LexemType.Not },
                { ".", LexemType.Dot },
                { "&", LexemType.Band },
                { "|", LexemType.Bor },
                { "~", LexemType.Binv },
                { "^", LexemType.Bxor },
                { "->", LexemType.Lambda },
                { "=", LexemType.Assignment },
                { "==", LexemType.Eqv },
                { ">", LexemType.Greater },
                { ">=", LexemType.GreaterEq },
                { "<", LexemType.Less },
                { "<=", LexemType.LessEq },
                { "!=", LexemType.NotEqv },
                { "<<", LexemType.BLshift },
                { ">>", LexemType.BRshift },
                { "+=", LexemType.PlusAssign },
                { "-=", LexemType.MinusAssign },
                { "*=", LexemType.MulAssign },
                { "/=", LexemType.TrueDivAssign },
                { "//=", LexemType.DivAssign },
                { "%=", LexemType.ModAssign },
                { "&=", LexemType.BandAssign },
                { "|=", LexemType.BorAssign },
                { "^=", LexemType.BxorAssign },
                { "<<=", LexemType.BLshiftAssign },
                { ">>=", LexemType.BRshiftAssign },
                { "&&", LexemType.And },
                { "&&=", LexemType.AndAssign },
                { "||", LexemType.Or },
                { "||=", LexemType.OrAssign },
                { ":", LexemType.Colon },
            };

        // Множество всех ключевых слов, после которого не может идти пробел
        private HashSet<LexemType> SpaceForbiddenKeywords { get; } = new HashSet<LexemType>()
            { LexemType.Operator, LexemType.Conversion };

        /// <summary>
        /// Множество всех пробельных символов (кроме \n - он рассматривается отдельно)
        /// </summary>
        private static readonly HashSet<char> WhiteSpace = new HashSet<char> { '\t', '\r', '\x0b', '\x0c', ' ' };

        /// <summary>
        /// Множество всех специальных символов - тех символов, которые встречаются в операторах
        /// (в идентификаторах их использовать нельзя)
        /// </summary>
        private static readonly HashSet<char> SpecialSymbols = new HashSet<char>();

        /// <summary>
        /// Множестов всех символов, ограничивающих строковый литерал (то есть кавычек)
        /// </summary>
        private static readonly HashSet<char> StringLiteralSymbols = new HashSet<char>() { '\'', '"' };

        static Lexer()
        {
            FillSpecialSymbols();
        }

        private static void FillSpecialSymbols() // Заполняет множество специальных символов
        {
            foreach (string s in NonTerminatingOperators.Keys)
            {
                foreach (char c in s)
                {
                    SpecialSymbols.Add(c);
                }
            }

            foreach (string s in TerminatingOperators.Keys)
            {
                foreach (char c in s)
                {
                    SpecialSymbols.Add(c);
                }
            }
        }

        private StreamFilter CharStream { get; } // Поток символов
        private string Filename { get; } // Название файла
        private int SymNum => CharStream.SymNumber;
        private int LineNum => CharStream.LineNumber;
        private string[] Lines { get; } // Массив строк файла - для красивых сообщений в ошибках

        // ReSharper disable once MemberCanBePrivate.Global
        public Lexer(string filename, string source)
        {
            Stream str = GenerateStreamFromString(source);
            Filename = filename;
            CharStream = new StreamFilter(str);
            Lines = source.Split("\n");
        }

        /// <summary>
        /// Производит лексический анализ кода из файла
        /// </summary>
        /// <param name="filename">Название файла для анализа</param>
        public Lexer(string filename)
            : this(filename, ReadFile(filename))
        {
        }

        private static string ReadFile(string path)
        {
            StringBuilder sb = new();
            using (var i = new StreamReader(File.OpenRead(path)))
            {
                while (!i.EndOfStream)
                {
                    sb.Append(i.ReadLine() + '\n');
                }
            }

            // Заменяем \r\n и \r на \n, чтобы дальше с ними проблем не было
            return sb.ToString().Replace("\r\n", "\n").Replace("\r", "\n");
        }

        static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private Lexem GetNumericLiteral() // Пытается прочесть в потоке числовой литерал
        {
            int lineStart = LineNum;
            int symStart = SymNum;

            LexemType type = LexemType.IntLiteral; // Изначально литерал целочисленный
            StringBuilder sb = new StringBuilder();

            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();
                if (c == '.') // Если нашли точку...
                {
                    if (type == LexemType.FloatLiteral) // ...не в первый раз - то литерал введен с ошибкой...
                    {
                        throw new InvalidNumericalLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                    }

                    type = LexemType.FloatLiteral; // Иначе меняем тип на вещественный
                    sb.Append(c);
                    CharStream.Advance();
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    CharStream.Advance();
                }
                else if (WhiteSpace.Contains(c) || SpecialSymbols.Contains(c) || c == '\n' ||
                         StringLiteralSymbols.Contains(c)) // В таких случаях прекращаем
                {
                    break;
                }
                else // Если нашли обычный символ (например, букву) - то литерал введен с ошибкой
                {
                    throw new InvalidNumericalLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                }
            }

            // Возвращаем литерал нужного типа
            if (type == LexemType.IntLiteral)
            {
                return new IntLiteral(sb.ToString(), lineStart, symStart);
            }

            return new FloatLiteral(sb.ToString(), lineStart, symStart);
        }

        private Lexem GetStringLiteral() // Пытается прочесть в потоке строковый литерал
        {
            int symStart = SymNum;
            int lineStart = LineNum;
            char start = CharStream.Peek();
            CharStream.Advance(); // Первую кавычку сразу вытаскиваем из потока
            char prev = start;
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();
                if (c == start && prev != '\\') // Если нашлась неэкранированная кавычка того же типа, что и начальная,
                    // то она означает конец строкового литерала
                {
                    CharStream.Advance();
                    return new StringLiteral(sb.ToString() + c, lineStart, symStart);
                }

                if (c == '\n') // Если литерал закончился до закрытия, он написан с ошибкой
                {
                    throw new UnterminatedStringLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                }

                sb.Append(c);
                prev = c;
                CharStream.Advance();
            }

            // До сюда дойдёт, если поток символов закончится, а значит строка не была закрыта
            // Если литерал закончился до закрытия, он написан с ошибкой
            throw new UnterminatedStringLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
        }

        // Пытается поделить последовательнотсть специальных символов на 2 оператора
        private IEnumerable<Lexem> DivideOperator(string res, int lineStart, int symStart)
        {
            for (int i = 0; i < res.Length; ++i)
            {
                string s1 = res.Substring(0, i + 1);
                string s2 = res.Substring(i + 1);
                if (NonTerminatingOperators.TryGetValue(s1, out var operator1) &&
                    NonTerminatingOperators.TryGetValue(s2, out var operator2))
                {
                    yield return new Lexem(operator1, lineStart, symStart);
                    yield return new Lexem(operator2, lineStart, symStart + i + 1);
                    yield break;
                }
            }

            throw new InvalidOperatorException(Filename, lineStart, symStart, Lines[lineStart - 1]);
        }

        private IEnumerable<Lexem> GetOperatorLexem() // Пытается прочесть оператор из потока
        {
            int lineStart = LineNum;
            int symStart = SymNum;

            StringBuilder sb = new StringBuilder();
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (!SpecialSymbols.Contains(c) || TerminatingOperators.ContainsKey(c.ToString()))
                    // Если последовательность специальных символов прервана
                {
                    if (sb.ToString() == "") // Обрабатываем прерывающий оператор
                    {
                        Debug.Assert(TerminatingOperators.ContainsKey(c.ToString()));
                        CharStream.Advance();
                        yield return new Lexem(TerminatingOperators[c.ToString()], lineStart, symStart);
                        yield break;
                    }

                    if (NonTerminatingOperators.ContainsKey(sb.ToString()))
                    {
                        // Если получился оператор, возвращаем его...
                        yield return new Lexem(NonTerminatingOperators[sb.ToString()], lineStart, symStart);
                        yield break;
                    }

                    // ...Если нет - пытаемся поделить получившуюся строку на 2
                    string res = sb.ToString();

                    foreach (Lexem lexem in DivideOperator(res, lineStart, symStart))
                    {
                        yield return lexem;
                    }

                    yield break;
                }

                sb.Append(c);
                CharStream.Advance();
            }

            // Если получился оператор, возвращаем его...
            if (NonTerminatingOperators.ContainsKey(sb.ToString()))
            {
                yield return new Lexem(NonTerminatingOperators[sb.ToString()], lineStart, symStart);
                yield break;
            }

            // ...Если нет - пытаемся поделить получившуюся строку на 2
            string res1 = sb.ToString();
            foreach (Lexem lexem in DivideOperator(res1, lineStart, symStart))
            {
                yield return lexem;
            }
        }

        private Lexem GetKeywordOrIdentifier() // Пытается прочесть ключевое слово из потока
        {
            int lineStart = LineNum;
            int symStart = SymNum;
            StringBuilder sb = new StringBuilder();
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (SpecialSymbols.Contains(c) || WhiteSpace.Contains(c) || c == '\n' ||
                    StringLiteralSymbols.Contains(c)) // Эти символы прерывают последовательность символов
                    // для ключевых слов и идентификаторов
                {
                    // Проверяем, что получилось - ключевое слово или идентификатор 
                    if (!KeyWords.ContainsKey(sb.ToString()))
                    {
                        return new Identifier(sb.ToString(), lineStart, symStart);
                    }

                    LexemType kw = KeyWords[sb.ToString()];
                    if (SpaceForbiddenKeywords.Contains(kw) && WhiteSpace.Contains(c))
                    {
                        throw new UnexpectedSpaceException(Filename, lineStart, SymNum, Lines[lineStart - 1]);
                    }

                    return new Lexem(kw, lineStart, symStart);
                }

                sb.Append(c);
                CharStream.Advance();
            }

            // Проверяем, что получилось - ключевое слово или идентификатор 
            if (!KeyWords.ContainsKey(sb.ToString()))
            {
                return new Identifier(sb.ToString(), lineStart, symStart);
            }

            return new Lexem(KeyWords[sb.ToString()], lineStart, symStart);
        }

        public IEnumerable<Lexem> Lex() // Производит лексический анализ
        {
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (WhiteSpace.Contains(c)) // Пробелы пропускаем
                {
                    CharStream.Advance();
                    continue;
                }

                if (c == '\n') // Переход строки - тоже лексема
                {
                    yield return new Lexem(LexemType.NewLine, LineNum, SymNum);
                    CharStream.Advance();
                    continue;
                }

                if (char.IsDigit(c) /*|| c == '.'*/) // Если лексема начинается с цифры - это числовой литерал
                {
                    yield return GetNumericLiteral();
                    continue;
                }

                if (StringLiteralSymbols.Contains(c)) // Если лексема начинается с кавычки - это строковый литерал
                {
                    yield return GetStringLiteral();
                    continue;
                }

                if (SpecialSymbols.Contains(c)) // Если лексема начинается со специального символа - это оператор
                {
                    foreach (Lexem lexem in GetOperatorLexem())
                    {
                        yield return lexem;
                    }

                    continue;
                }

                // Иначе, это ключевое слово или идентификатор
                yield return GetKeywordOrIdentifier();
            }
        }

        ~Lexer()
        {
            CharStream.Close();
        }
    }
}