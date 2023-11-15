using System.Collections;
using SyntaxAnalyzer.Nodes;

namespace SyntaxTest;

public interface ITreeTester
{
    public bool Test(INode? node);
}

public delegate bool FieldsTester(dynamic x);

public class TreeTester<T> : ITreeTester
    where T : INode
{
    private FieldsTester? Tester { get; }
    private ITreeTester[] ChildrenTesters { get; }

    public TreeTester(FieldsTester? fieldsTester = null, params ITreeTester[] testers)
    {
        Tester = fieldsTester;
        ChildrenTesters = testers;
    }

    public bool Test(INode? node)
    {
        if (node is not T)
        {
            return false;
        }

        if (Tester != null && !Tester!.Invoke(node))
        {
            return false;
        }

        int n = ChildrenTesters.Length;
        int i = 0;

        foreach (INode? node1 in node.Walk())
        {
            if (i >= n)
            {
                return false;
            }

            if (!ChildrenTesters[i].Test(node1))
            {
                return false;
            }
        }

        return i == n;
    }
}

public class NullTester : ITreeTester
{
    public bool Test(INode? node) => node == null;
}