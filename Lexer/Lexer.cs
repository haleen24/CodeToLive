using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lexer
{
    public class Lexer
    {
        private static Dictionary<string, LexemType> keyWords = new Dictionary<string, LexemType>
        {
            { "if", LexemType.If },
            { "while", LexemType.While },
            { "for", LexemType.For },
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

        private static Dictionary<string, LexemType> operators = new Dictionary<string, LexemType>
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
            { ">>", LexemType.BRshift }
        };

        private static CharTree keyWordsTree;
        private static CharTree operatorsTree;

        private static HashSet<char> whiteSpace = new HashSet<char> { '\t', '\n', '\r', '\x0b', '\x0c', ' ' };
        private static HashSet<char> specialSymbols = new HashSet<char>();

        static Lexer()
        {
            keyWordsTree = new CharTree(keyWords.Keys);
            operatorsTree = new CharTree(operators.Keys);
            FillSpecialSymbols();
        }

        private static void FillSpecialSymbols()
        {
            foreach (string s in operators.Keys)
            {
                foreach (char c in s)
                {
                    specialSymbols.Add(c);
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

        private Lexem GetNumericLiteral(StreamReader stream)
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
                else if (whiteSpace.Contains(c) || specialSymbols.Contains(c))
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

        private Lexem GetStringLiteral(StreamReader stream)
        {
            char prev = (char)stream.Read();
            StringBuilder sb = new StringBuilder(prev);
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (c == '\'' && prev != '\\')
                {
                    stream.Read();
                    return new StringLiteral(sb.ToString() + c);
                }

                if (c == '\n' || c == '\r')
                {
                    throw new Exception("Message"); // TODO: Система исключений
                }

                sb.Append(c);
                prev = c;
                stream.Read();
            }

            throw new Exception("Message"); // TODO: Система исключений
        }

        private Lexem GetOperatorLexem(StreamReader stream)
        {
            String sb = "";
            CharTree current = operatorsTree;
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();

                if (!specialSymbols.Contains(c))
                {
                    if (sb != current.Value)
                    {
                        throw new Exception("Message"); // TODO: Система исключений
                    }

                    return new Lexem(operators[sb]);
                }

                if (sb.Length == current.Value.Length)
                {
                    CharTree pc = current.PrefixChild(sb);
                    if (pc == null)
                    {
                        throw new Exception("Message"); // TODO: Система исключений
                    }

                    current = pc;
                    sb += c;
                    stream.Read();
                }
            }
            throw new Exception("Message"); // TODO: Система исключений
        }

        private Lexem GetKeywordOrIdentifier(StreamReader stream)
        {
        }

        public IEnumerable<Lexem> Lex(string str)
        {
            using (var stream = new StreamReader(GenerateStreamFromString(str)))
            {
                while (!stream.EndOfStream)
                {
                    char c = (char)stream.Peek();

                    if (whiteSpace.Contains(c))
                    {
                        stream.Read();
                        continue;
                    }

                    if (char.IsDigit(c) || c == '.')
                    {
                        yield return GetNumericLiteral(stream);
                        continue;
                    }

                    if (c == '\'')
                    {
                        yield return GetStringLiteral(stream);
                        continue;
                    }

                    if (specialSymbols.Contains(c))
                    {
                        yield return GetOperatorLexem(stream);
                        continue;
                    }

                    yield return GetKeywordOrIdentifier(stream);
                }
            }
        }
    }
}