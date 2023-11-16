namespace SyntaxAnalyzer.Nodes;

public class Setter : EmptyNode, INode  // Если аттрибутом оказался setter, то в Attribute нужно подставлять не StaticLexemNode
                             // (потому что их не должно быть в итоговм дереве), а объект этого класса
                             // Не путать с SetterDeclaration! - это для функций
{
    public IEnumerable<INode?> Walk()
    {
        yield break;
    }
}