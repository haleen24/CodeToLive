using NUnit;
using Lexem = LexerSpace.Lexem;

namespace LexerTest
{
    [TestFixture]
    public class Tests
    {
        private List<string> _fileNames = new List<string>() { "test_1", "test_2" };

        private List<List<Lexem>> _answers = new List<List<Lexem>>();

        private global::LexerSpace.Lexer _lexer;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLexerFromFiles()
        {
            string str;
            for (int i = 0; i < _fileNames.Count; ++i)
            {
                _lexer = new(_fileNames[i]);
                Assert.AreEqual(_answers[i], _lexer.Lex());
            }
        }
        
    }
}