using LexerSpace;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Parsers;
using SyntaxAnalyzer.Rules;

namespace SyntaxAnalyzer;

internal static class LexemStreamConstructor
{
    public static IEnumerable<Lexem> RemoveDuplicateNewLines(IEnumerable<Lexem> input)
    {
        LexemType? prev = null;

        foreach (Lexem lexem in input)
        {
            if (!(prev == LexemType.NewLine && lexem.LType == LexemType.NewLine))
            {
                yield return lexem;
            }

            prev = lexem.LType;
        }
    }

    public static IEnumerable<Lexem> ConcatLines(IEnumerable<Lexem> input)
    {
        LexemType? prev = null;

        foreach (Lexem lexem in input)
        {
            if (prev == LexemType.LineConcat)
            {
                if (lexem.LType != LexemType.NewLine)
                {
                    throw new Exception("\\ error"); // TODO: exceptions
                }
            }
            else if (lexem.LType != LexemType.LineConcat)
            {
                yield return lexem;
            }

            prev = lexem.LType;
        }
    }

    public static LexemStream ConstructStream(IEnumerable<Lexem> input)
    {
        var lexems = new List<Lexem>(ConcatLines(RemoveDuplicateNewLines(input)));
        return new LexemStream(lexems);
    }
}

public class Syntaxer
{
    private GrammarUnit Main { get; }

    public Syntaxer(GrammarUnit main)
    {
        Main = main;
    }

    private INode ParseStream(LexemStream ls)
    {
        IParser parser = RulesMap.GetParser(Main);

        if (!parser.Parse(ls))
        {
            throw new Exception("Syntax error");  // TODO: exceptions
        }

        return RulesMap.GetNode(Main, parser);
    }

    public INode Parse(IEnumerable<Lexem> input)
    {
        LexemStream ls = LexemStreamConstructor.ConstructStream(input);
        return ParseStream(ls);
    }
}