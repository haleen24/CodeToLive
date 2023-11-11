using System.Text;
using Lexer.Exceptions;

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
            foreach (string s in NonTerminatingOperators.Keys)
            {
                foreach (char c in s)
                {
                    _specialSymbols.Add(c);
                }
            }

            foreach (string s in TerminatingOperators.Keys)
            {
                foreach (char c in s)
                {
                    _specialSymbols.Add(c);
                }
            }
        }

        private StreamReader _stream;
        private string _filename;
        private int _symNum;
        private int _lineNum;
        private StringBuilder _lastLine;

        public Lexer(string filename, string source)
        {
            Stream str = GenerateStreamFromString(source);
            _filename = filename;
            _stream = new StreamReader(str);
            _symNum = 1;
            _lineNum = 1;
            _lastLine = new StringBuilder();
        }

        private void Advance()
        {
            char c = (char)_stream.Read();
            if (c == '\n')
            {
                _symNum = 1;
                _lineNum += 1;
                _lastLine.Clear();
            }
            else
            {
                _symNum += 1;
                _lastLine.Append(c);
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

        private Lexem GetNumericLiteral(StreamReader stream)
        {
            int lineStart = _lineNum;
            int symStart = _symNum;

            LexemType type = LexemType.IntLiteral;
            StringBuilder sb = new StringBuilder();

            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (c == '.')
                {
                    if (type == LexemType.FloatLiteral)
                    {
                        throw new InvalidNumericalLiteralException(_filename, lineStart, symStart, _lastLine.Append(c));
                    }

                    type = LexemType.FloatLiteral;
                    sb.Append(c);
                    Advance();
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    Advance();
                }
                else if (WhiteSpace.Contains(c) || _specialSymbols.Contains(c) || c == '\n' ||
                         StringLiteralSymbols.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new InvalidNumericalLiteralException(_filename, lineStart, symStart, _lastLine.Append(c));
                }
            }

            if (type == LexemType.IntLiteral)
            {
                return new IntLiteral(sb.ToString());
            }

            return new FloatLiteral(sb.ToString());
        }

        private Lexem GetStringLiteral(StreamReader stream)
        {
            int symStart = _symNum;
            int lineStart = _lineNum;
            char start = (char)stream.Peek();
            Advance();
            char prev = start;
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (c == start && prev != '\\')
                {
                    Advance();
                    return new StringLiteral(sb.ToString() + c);
                }

                if (c == '\n')
                {
                    throw new UnterminatedStringLiteralException(_filename, lineStart, symStart, _lastLine);
                }

                sb.Append(c);
                prev = c;
                Advance();
            }

            throw new UnterminatedStringLiteralException(_filename, lineStart, symStart, _lastLine);
        }

        private IEnumerable<Lexem> DivideOperator(string res, int lineStart, int symStart, string append)
        {
            for (int i = 0; i < res.Length; ++i)
            {
                string s1 = res.Substring(0, i + 1);
                string s2 = res.Substring(i + 1);
                if (NonTerminatingOperators.TryGetValue(s1, out var operator1) &&
                    NonTerminatingOperators.TryGetValue(s2, out var operator2))
                {
                    yield return new Lexem(operator1);
                    yield return new Lexem(operator2);
                    yield break;
                }
            }
            throw new InvalidOperatorException(_filename, lineStart, symStart, _lastLine.Append(append));
        }

        private IEnumerable<Lexem> GetOperatorLexem(StreamReader stream)
        {
            int lineStart = _lineNum;
            int symStart = _symNum;

            StringBuilder sb = new StringBuilder();
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();

                if (!_specialSymbols.Contains(c) || TerminatingOperators.ContainsKey(c.ToString()))
                {
                    if (sb.ToString() == "")
                    {
                        Advance();
                        yield return new Lexem(TerminatingOperators[c.ToString()]);
                        yield break;
                    }

                    if (NonTerminatingOperators.ContainsKey(sb.ToString()))
                    {
                        yield return new Lexem(NonTerminatingOperators[sb.ToString()]);
                        yield break;
                    }

                    string res = sb.ToString();

                    foreach (Lexem lexem in DivideOperator(res, lineStart, symStart, c.ToString()))
                    {
                        yield return lexem;
                    }
                    yield break;
                }

                sb.Append(c);
                Advance();
            }

            if (!NonTerminatingOperators.ContainsKey(sb.ToString()))
            {
                yield return new Lexem(NonTerminatingOperators[sb.ToString()]);
                yield break;
            }

            string res1 = sb.ToString();
            foreach (Lexem lexem in DivideOperator(res1, lineStart, symStart, ""))
            {
                yield return lexem;
            }
        }

        private Lexem GetKeywordOrIdentifier(StreamReader stream)
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
                Advance();
            }

            if (!KeyWords.ContainsKey(sb.ToString()))
            {
                return new Identifier(sb.ToString());
            }

            return new Lexem(KeyWords[sb.ToString()]);
        }

        public IEnumerable<Lexem> Lex()
        {
            while (!_stream.EndOfStream)
            {
                char c = (char)_stream.Peek();

                if (WhiteSpace.Contains(c))
                {
                    Advance();
                    continue;
                }

                if (c == '\n')
                {
                    yield return new Lexem(LexemType.NewLine);
                    Advance();
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    yield return GetNumericLiteral(_stream);
                    continue;
                }

                if (StringLiteralSymbols.Contains(c))
                {
                    yield return GetStringLiteral(_stream);
                    continue;
                }

                if (_specialSymbols.Contains(c))
                {
                    foreach (Lexem lexem in GetOperatorLexem(_stream))
                    {
                        yield return lexem;
                    }
                    continue;
                }

                yield return GetKeywordOrIdentifier(_stream);
            }
        }

        ~Lexer()
        {
            _stream.Close();
        }
    }
}