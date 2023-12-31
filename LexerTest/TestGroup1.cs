using LexerSpace;

namespace LexerTest
{
    [TestFixture]
    public class TestGroup1
    {
        private List<string> FileNames { get; } = new List<string>()
        {
            "test_1.txt",
            "test_2.txt",
            "test_3.txt",
            "test_4.txt",
            "KeywordsTest.txt",
            "OperatorsTest.txt"
        };

        private List<string> Answers { get; } = new List<string>()
        {
            "answer_1.txt",
            "answer_2.txt",
            "answer_3.txt",
            "answer_4.txt",
            "KeywordsAnswer.txt",
            "OperatorsAnswer.txt"
        };

        [SetUp]
        public void Setup()
        {
        }

        private void TestTemplate(int i)
        {
            string path = "../../../TestFiles/";
            Lexer lexer = new(path + FileNames[i]);
            List<string> res = new();
            foreach (var lexem in lexer.Lex())
            {
                res.Add(lexem.ToString());
            }

            Assert.That(res, Is.EqualTo(ReadAnswers(path + Answers[i])));
        }

        [Test]
        public void Test1()
        {
            TestTemplate(0);
        }

        [Test]
        public void Test2()
        {
            TestTemplate(1);
        }

        [Test]
        public void Test3()
        {
            TestTemplate(2);
        }

        [Test]
        public void Test4()
        {
            TestTemplate(3);
        }

        [Test]
        public void Test5()
        {
            TestTemplate(4);
        }

        [Test]
        public void Test6()
        {
            TestTemplate(5);
        }

        private List<string> ReadAnswers(string path)
        {
            List<string> list = new();
            using var i = new StreamReader(File.OpenRead(path));
            while (!i.EndOfStream)
            {
                list.Add(i.ReadLine()!);
            }

            return list;
        }
    }
}