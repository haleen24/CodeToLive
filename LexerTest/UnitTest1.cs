using NUnit;
using LexerSpace;
using System.Collections.Generic;

namespace LexerTest
{
    [TestFixture]
    public class Tests
    {
        private List<string> _fileNames = new List<string>()
        {
            "test_1.txt",
            "test_2.txt",
            "test_3.txt",
            "test_4.txt",
            "KeywordsTest.txt",
            "OperatorsTest.txt"
        };

        private List<string> _answers = new List<string>()
        {
            "answer_1.txt",
            "answer_2.txt",
            "answer_3.txt",
            "answer_4.txt",
            "KeywordsAnswer.txt",
            "OperatorsAnswer.txt"
        };

        private Lexer _lexer;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLexerFromFiles()
        {
            string path = "../../../TestFiles/";
            for (int i = 0; i < _fileNames.Count; ++i)
            {
                _lexer = new(path + _fileNames[i]);
                List<string> res = new();
                foreach (var lexem in _lexer.Lex())
                {
                    res.Add(lexem.ToString());
                }

                Assert.AreEqual(ReadAnswers(path + _answers[i]), res);
            }
        }

        private List<string> ReadAnswers(string path)
        {
            List<string> list = new();
            using (var i = new StreamReader(File.OpenRead(path)))
            {
                while (!i.EndOfStream)
                {
                    list.Add(i.ReadLine());
                }
            }

            return list;
        }
    }
}