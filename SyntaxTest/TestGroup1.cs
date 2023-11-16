using System.Collections.ObjectModel;
using LexerSpace;
using SyntaxAnalyzer;
using SyntaxAnalyzer.Nodes;
using SyntaxAnalyzer.Rules;
using Identifier = SyntaxAnalyzer.Nodes.Identifier;
using IntLiteral = LexerSpace.IntLiteral;
using StringLiteral = SyntaxAnalyzer.Nodes.StringLiteral;

namespace SyntaxTest;

public class TestGroup1
{
    private IList<IEnumerable<Lexem>> LexerOutputs { get; } = new List<IEnumerable<Lexem>>();
    private IList<ITreeTester> Testers { get; } = new List<ITreeTester>();

    private ITreeTester TT<T>(params ITreeTester[] testers) where T : INode
    {
        return new TreeTester<T>(testers);
    }

    private ITreeTester TT<T>(FieldsTester? ft, params ITreeTester[] testers) where T : INode
    {
        return new TreeTester<T>(ft, testers);
    }

    private ITreeTester N() => new NullTester();

    private ITreeTester FizzBuzz()
    {
        return TT<Module>(
            TT<FunctionDefinition>(
                ft: null,
                TT<Identifier>(o => o.Value == "fizz_buzz"),
                TT<Identifier>(o => o.Value == "n"),
                TT<Block>(
                    ft: null,
                    TT<If>(
                        ft: null,
                        TT<BinaryExpression>(
                            o => o.Operator == LexemType.Eqv,
                            TT<BinaryExpression>(
                                o => o.Operator == LexemType.Mod,
                                TT<Identifier>(o => o.Value == "n"),
                                TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "15")
                            ),
                            TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "0")
                        ),
                        TT<Return>(
                            ft: null,
                            TT<StringLiteral>(o => o.Value == "\"FizzBuzz\"")
                        ),
                        N()
                    ),
                    TT<If>(
                        ft: null,
                        TT<BinaryExpression>(
                            o => o.Operator == LexemType.Eqv,
                            TT<BinaryExpression>(
                                o => o.Operator == LexemType.Mod,
                                TT<Identifier>(o => o.Value == "n"),
                                TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "5")
                            ),
                            TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "0")
                        ),
                        TT<Block>(
                            ft: null,
                            TT<Return>(
                                ft: null,
                                TT<StringLiteral>(o => o.Value == "\"Buzz\"")
                            )
                        ),
                        TT<If>(
                            ft: null,
                            TT<BinaryExpression>(
                                o => o.Operator == LexemType.Eqv,
                                TT<BinaryExpression>(
                                    o => o.Operator == LexemType.Mod,
                                    TT<Identifier>(o => o.Value == "n"),
                                    TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "3")
                                ),
                                TT<SyntaxAnalyzer.Nodes.IntLiteral>(o => o.Value == "0")
                            ),
                            TT<Return>(
                                ft: null,
                                TT<StringLiteral>(o => o.Value == "\"Fizz\"")
                            ),
                            TT<Return>(
                                ft: null,
                                TT<Identifier>(o => o.Value == "n")
                            )
                        )
                    )
                )
            ),
            TT<FunctionCall>(
                ft: null,
                TT<Identifier>(o => o.Value == "fizz_buzz"),
                TT<FunctionCall>(
                    ft: null,
                    TT<Identifier>(o => o.Value == "int"),
                    TT<FunctionCall>(
                        ft: null,
                        TT<Identifier>(o => o.Value == "input")
                    )
                )
            )
        );
    }

    [SetUp]
    public void Setup()
    {
        string[] filenames = new[]
            { "fizzbuzz.upl", "binary_tree.up", "matrix_storage.upl", "reduce.upl", "streams.upl", "vector2.upl" };

        foreach (string s in filenames)
        {
            string fn = "../../../TestInputs" + s;
            Lexer l = new Lexer(fn);
            LexerOutputs.Add(l.Lex());
        }
        
        Testers.Add(FizzBuzz());
    }

    [TestCase(0)]
    public void TestAst(int ind)
    {
        Syntaxer syntaxer = new Syntaxer(new GrammarUnit(GrammarUnitType.Module));
        INode res = syntaxer.Parse(LexerOutputs[ind]);
        ITreeTester tester = Testers[ind];
        Assert.That(tester.Test(res));
    }
}