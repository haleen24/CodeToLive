using System.Text;
using LexerSpace.Exceptions;

namespace LexerSpace
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

        private StreamFilter CharStream { get; }
        private string Filename { get; }
        private int SymNum => CharStream.SymNumber;
        private int LineNum => CharStream.LineNumber;
        private string[] Lines { get; }

        public Lexer(string filename, string source)
        {
            Stream str = GenerateStreamFromString(source);
            Filename = filename;
            CharStream = new StreamFilter(str);
            Lines = source.Split("\n");
        }

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

        private Lexem GetNumericLiteral()
        {
            int lineStart = LineNum;
            int symStart = SymNum;

            LexemType type = LexemType.IntLiteral;
            StringBuilder sb = new StringBuilder();

            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();
                if (c == '.')
                {
                    if (type == LexemType.FloatLiteral)
                    {
                        throw new InvalidNumericalLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                    }

                    type = LexemType.FloatLiteral;
                    sb.Append(c);
                    CharStream.Advance();
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    CharStream.Advance();
                }
                else if (WhiteSpace.Contains(c) || _specialSymbols.Contains(c) || c == '\n' ||
                         StringLiteralSymbols.Contains(c))
                {
                    break;
                }
                else
                {
                    throw new InvalidNumericalLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                }
            }

            if (type == LexemType.IntLiteral)
            {
                return new IntLiteral(sb.ToString(), lineStart, symStart);
            }

            return new FloatLiteral(sb.ToString(), lineStart, symStart);
        }

        private Lexem GetStringLiteral()
        {
            int symStart = SymNum;
            int lineStart = LineNum;
            char start = CharStream.Peek();
            CharStream.Advance();
            char prev = start;
            StringBuilder sb = new StringBuilder();
            sb.Append(start);
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();
                if (c == start && prev != '\\')
                {
                    CharStream.Advance();
                    return new StringLiteral(sb.ToString() + c, lineStart, symStart);
                }

                if (c == '\n')
                {
                    throw new UnterminatedStringLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
                }

                sb.Append(c);
                prev = c;
                CharStream.Advance();
            }

            throw new UnterminatedStringLiteralException(Filename, lineStart, symStart, Lines[lineStart - 1]);
        }

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

        private IEnumerable<Lexem> GetOperatorLexem()
        {
            int lineStart = LineNum;
            int symStart = SymNum;

            StringBuilder sb = new StringBuilder();
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (!_specialSymbols.Contains(c) || TerminatingOperators.ContainsKey(c.ToString()))
                {
                    if (sb.ToString() == "")
                    {
                        CharStream.Advance();
                        yield return new Lexem(TerminatingOperators[c.ToString()], lineStart, symStart);
                        yield break;
                    }

                    if (NonTerminatingOperators.ContainsKey(sb.ToString()))
                    {
                        yield return new Lexem(NonTerminatingOperators[sb.ToString()], lineStart, symStart);
                        yield break;
                    }

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

            if (NonTerminatingOperators.ContainsKey(sb.ToString()))
            {
                yield return new Lexem(NonTerminatingOperators[sb.ToString()], lineStart, symStart);
                yield break;
            }

            string res1 = sb.ToString();
            foreach (Lexem lexem in DivideOperator(res1, lineStart, symStart))
            {
                yield return lexem;
            }
        }

        private Lexem GetKeywordOrIdentifier()
        {
            int lineStart = LineNum;
            int symStart = SymNum;
            StringBuilder sb = new StringBuilder();
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (_specialSymbols.Contains(c) || WhiteSpace.Contains(c) || c == '\n' ||
                    StringLiteralSymbols.Contains(c))
                {
                    if (!KeyWords.ContainsKey(sb.ToString()))
                    {
                        return new Identifier(sb.ToString(), lineStart, symStart);
                    }

                    return new Lexem(KeyWords[sb.ToString()], lineStart, symStart);
                }

                sb.Append(c);
                CharStream.Advance();
            }

            if (!KeyWords.ContainsKey(sb.ToString()))
            {
                return new Identifier(sb.ToString(), lineStart, symStart);
            }

            return new Lexem(KeyWords[sb.ToString()], lineStart, symStart);
        }

        public IEnumerable<Lexem> Lex()
        {
            while (!CharStream.EndOfStream)
            {
                char c = CharStream.Peek();

                if (WhiteSpace.Contains(c))
                {
                    CharStream.Advance();
                    continue;
                }

                if (c == '\n')
                {
                    yield return new Lexem(LexemType.NewLine, LineNum, SymNum);
                    CharStream.Advance();
                    continue;
                }

                if (char.IsDigit(c) /*|| c == '.'*/)
                {
                    yield return GetNumericLiteral();
                    continue;
                }

                if (StringLiteralSymbols.Contains(c))
                {
                    yield return GetStringLiteral();
                    continue;
                }

                if (_specialSymbols.Contains(c))
                {
                    foreach (Lexem lexem in GetOperatorLexem())
                    {
                        yield return lexem;
                    }

                    continue;
                }

                yield return GetKeywordOrIdentifier();
            }
        }

        ~Lexer()
        {
            CharStream.Close();
        }
    }
}