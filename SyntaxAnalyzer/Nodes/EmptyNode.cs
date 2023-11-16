namespace SyntaxAnalyzer.Nodes;

public class EmptyNode
{
    private string TypeName => GetType().ToString().Split('.')[^1];
    
    public override string ToString()
    {
        return $"{TypeName}()";
    }
}