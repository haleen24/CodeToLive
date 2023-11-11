using System.Text;

namespace Lexer
{
    public class Lexer
    {
        private static readonly Dictionary<string, LexemType> KeyWords = new Dictionary<string, LexemType>
        {
            { "if", LexemType.If },
            { "while", LexemType.While },
            { "for", LexemType.For },
            { "foreach", LexemType.Foreach },
            { "func", LexemType.Func },
            { "class", LexemType.Class },
            { "true", LexemType.True },
            { "false", LexemType.False },
            { "null", LexemType.Null },
            { "is", LexemType.Is },
            { "break", LexemType.Break },
            { "return", LexemType.Return },
            { "continue", LexemType.Continue },
            { "else", LexemType.Else }
        };

        private static readonly Dictionary<string, LexemType> Operators = new Dictionary<string, LexemType>
        {
            { "+", LexemType.Plus },
            { "-", LexemType.Minus },
            { "*", LexemType.Product },
            { "/", LexemType.TrueDiv },
            { "//", LexemType.Div },
            { "%", LexemType.Mod },
            { "(", LexemType.Lparenthese },
            { ")", LexemType.Rparenthese },
            { "{", LexemType.Lbrace },
            { "}", LexemType.Rbrace },
            { "[", LexemType.Lbracket },
            { "]", LexemType.Rbracket },
            { "!", LexemType.Not },
            { ".", LexemType.Dot },
            { "&", LexemType.Band },
            { "|", LexemType.Bor },
            { "~", LexemType.Binv },
            { "^", LexemType.Bxor },
            { ",", LexemType.Comma },
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
            { "\\", LexemType.LineConcat },
            { ";", LexemType.Semicolon }
        };

        private static readonly HashSet<char> WhiteSpace = new HashSet<char> { '\t', '\r', '\x0b', '\x0c', ' ' };
        private static HashSet<char> _specialSymbols = new HashSet<char>();
        private static readonly HashSet<char> StringLiteralSymbols = new HashSet<char>() { '\'', '"' };

        static Lexer()
        {
            FillSpecialSymbols();
        }

        private static void FillSpecialSymbols()
        {
            foreach (string s in Operators.Keys)
            {
                foreach (char c in s)
                {
                    _specialSymbols.Add(c);
                }
            }
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

        private static Lexem GetNumericLiteral(StreamReader stream)
        {
            LexemType type = LexemType.IntLiteral;
            StringBuilder sb = new StringBuilder();

            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (c == '.')
                {
                    if (type == LexemType.FloatLiteral)
                    {
                        throw new Exception("Message"); // TODO: Система исключений
                    }

                    type = LexemType.FloatLiteral;
                    sb.Append(c);
                    stream.Read();
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    stream.Read();
                }
                else if (WhiteSpace.Contains(c) || _specialSymbols.Contains(c) || c == '\n' ||
                         StringLiteralSymbols.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new Exception("Message"); // TODO: Система исключений
                }
            }

            if (type == LexemType.IntLiteral)
            {
                return new IntLiteral(sb.ToString());
            }

            return new FloatLiteral(sb.ToString());
        }

        private static Lexem GetStringLiteral(StreamReader stream)
        {
            char start = (char)stream.Read();
            char prev = start;
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (c == start && prev != '\\')
                {
                    stream.Read();
                    return new StringLiteral(sb.ToString() + c);
                }

                if (c == '\n')
                {
                    throw new Exception("Message"); // TODO: Система исключений
                }

                sb.Append(c);
                prev = c;
                stream.Read();
            }

            throw new Exception("Message"); // TODO: Система исключений
        }

        private static Lexem GetOperatorLexem(StreamReader stream)
        {
            StringBuilder sb = new StringBuilder();
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();

                if (!_specialSymbols.Contains(c))
                {
                    if (!Operators.ContainsKey(sb.ToString()))
                    {
                        throw new Exception("Message"); // TODO: Система исключений
                    }

                    return new Lexem(Operators[sb.ToString()]);
                }

                sb.Append(c);
                stream.Read();
            }

            if (!Operators.ContainsKey(sb.ToString()))
            {
                throw new Exception("Message"); // TODO: Система исключений
            }

            return new Lexem(Operators[sb.ToString()]);
        }

        private static Lexem GetKeywordOrIdentifier(StreamReader stream)
        {
            StringBuilder sb = new StringBuilder();
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();

                if (_specialSymbols.Contains(c) || WhiteSpace.Contains(c) || c == '\n' ||
                    StringLiteralSymbols.Contains(c))
                {
                    if (!KeyWords.ContainsKey(sb.ToString()))
                    {
                        return new Identifier(sb.ToString());
                    }

                    return new Lexem(KeyWords[sb.ToString()]);
                }

                sb.Append(c);
                stream.Read();
            }

            if (!KeyWords.ContainsKey(sb.ToString()))
            {
                return new Identifier(sb.ToString());
            }

            return new Lexem(KeyWords[sb.ToString()]);
        }

        public IEnumerable<Lexem> Lex(string str)
        {
            using var stream = new StreamReader(GenerateStreamFromString(str));
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();

                if (WhiteSpace.Contains(c))
                {
                    stream.Read();
                    continue;
                }

                if (c == '\n')
                {
                    yield return new Lexem(LexemType.NewLine);
                    stream.Read();
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    yield return GetNumericLiteral(stream);
                    continue;
                }

                if (StringLiteralSymbols.Contains(c)) 
                {
                    yield return GetStringLiteral(stream);
                    continue;
                }

                if (_specialSymbols.Contains(c))
                {
                    yield return GetOperatorLexem(stream);
                    continue;
                }

                yield return GetKeywordOrIdentifier(stream);
            }
        }
    }
}