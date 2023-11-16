namespace SyntaxAnalyzer.Nodes;

public class Getter : EmptyNode, INode  // Если аттрибутом оказался getter, то в Attribute нужно подставлять не StaticLexemNode
                             // (потому что их не должно быть в итоговм дереве), а объект этого класса
                             // Не путать с GetterDeclaration! - это для функций
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}