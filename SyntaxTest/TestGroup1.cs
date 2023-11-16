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
                TT<Identifier>(o => o.Value == "fizz_buzz"),
                TT<Identifier>(o => o.Value == "n"),
                TT<Block>(
                    TT<If>(
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
                            TT<StringLiteral>(o => o.Value == "\"FizzBuzz\"")
                        ),
                        N()
                    ),
                    TT<If>(
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
                            TT<Return>(
                                TT<StringLiteral>(o => o.Value == "\"Buzz\"")
                            )
                        ),
                        TT<If>(
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
                                TT<StringLiteral>(o => o.Value == "\"Fizz\"")
                            ),
                            TT<Return>(
                                TT<Identifier>(o => o.Value == "n")
                            )
                        )
                    )
                )
            ),
            TT<FunctionCall>(
                TT<Identifier>(o => o.Value == "fizz_buzz"),
                TT<FunctionCall>(
                    TT<Identifier>(o => o.Value == "int"),
                    TT<FunctionCall>(
                        TT<Identifier>(o => o.Value == "input")
                    )
                )
            )
        );
    }

    private ITreeTester Reduce()
    {
        return TT<Module>(
            TT<FunctionDefinition>(
                o => o.ParamsArgument != null && o.PositionalArguments.Count == 1,
                TT<Identifier>(o => o.Value == "reduce"),
                TT<Identifier>(o => o.Value == "op"),
                TT<Identifier>(o => o.Value == "args"),
                TT<Block>(
                    TT<Assignment>(
                        o => !o.IsSeq && o.Sign == LexemType.Assignment,
                        TT<IdentifierWithFinal>(o => !o.IsFinal && o.Value == "res"),
                        TT<Null>()
                    ),
                    TT<Assignment>(
                        o => !o.IsSeq && o.Sign == LexemType.Assignment,
                        TT<IdentifierWithFinal>(o => !o.IsFinal && o.Value == "changed"),
                        TT<False>()
                    ),
                    TT<Foreach>(
                        TT<Identifier>(o => o.Value == "arg"),
                        TT<Identifier>(o => o.Value == "args"),
                        TT<Block>(
                            TT<If>(
                                TT<UnaryExpression>(
                                    o => o.Operator == LexemType.Not,
                                    TT<Identifier>(o => o.Value == "changed")
                                ),
                                TT<Block>(
                                    TT<Assignment>(
                                        o => !o.IsSeq && o.Sign == LexemType.Assignment,
                                        TT<IdentifierWithFinal>(o => !o.IsFinal && o.Value == "changed"),
                                        TT<True>()
                                    ),
                                    TT<Assignment>(
                                        o => !o.IsSeq && o.Sign == LexemType.Assignment,
                                        TT<IdentifierWithFinal>(o => !o.IsFinal && o.Value == "res"),
                                        TT<Identifier>(o => o.Value == "arg")
                                    ),
                                    TT<Continue>()
                                )
                            ),
                            TT<Assignment>(
                                o => !o.IsSeq && o.Sign == LexemType.Assignment,
                                TT<IdentifierWithFinal>(o => !o.IsFinal && o.Value == "res"),
                                TT<FunctionCall>(
                                    TT<Identifier>(o => o.Value == "op"),
                                    TT<Identifier>(o => o.Value == "res"),
                                    TT<Identifier>(o => o.Value == "arg")
                                )
                            )
                        )
                    ),
                    TT<Return>(
                        TT<Identifier>(o => o.Value == "Res")
                    )
                )
            )
        );
    }

    [SetUp]
    public void Setup()
    {
        string[] filenames = new[]
            { "fizzbuzz.upl", "reduce.upl", "matrix_storage.upl", "binary_tree.upl", "streams.upl", "vector2.upl" };

        foreach (string s in filenames)
        {
            string fn = "../../../TestInputs" + s;
            Lexer l = new Lexer(fn);
            LexerOutputs.Add(l.Lex());
        }
        
        Testers.Add(FizzBuzz());
        Testers.Add(Reduce());
    }

    [TestCase(0)]
    [TestCase(1)]
    public void TestAst(int ind)
    {
        Syntaxer syntaxer = new Syntaxer(new GrammarUnit(GrammarUnitType.Module));
        INode res = syntaxer.Parse(LexerOutputs[ind]);
        ITreeTester tester = Testers[ind];
        Assert.That(tester.Test(res));
    }
}