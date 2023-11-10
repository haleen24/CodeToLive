using System;
using System.Collections.Generic;

namespace Lexer
{
    public class CharTree
    {
        public string Value { get; private set; }
        public List<CharTree> Children { get; }
        
        public bool IsComplete { get; private set; }

        public CharTree() : this("")
        {
            IsComplete = false;
        }

        public CharTree(string val)
        {
            Value = val;
            IsComplete = true;
            Children = new List<CharTree>();
        }

        public CharTree(IEnumerable<string> vals) : this()
        {
            foreach (string s in vals.OrderBy(x => x.Length))
            {
                Add(s);
            }
        }

        private int BiggestCommonPrefix(string s1, string s2, int start)
        {
            int n = Math.Min(s1.Length, s2.Length);

            for (; start < n; ++start)
            {
                if (s1[start] != s2[start])
                {
                    break;
                }
            }

            return start;
        }

        private void Add(string str)
        {
            CharTree? toAdd = PrefixChild(str);
            if (toAdd == null)
            {
                int start = Value.Length;

                bool added = false;
                foreach (CharTree child in Children)
                {
                    string cv = child.Value;
                    int n = BiggestCommonPrefix(str, cv, start);
                    if (n > start)
                    {
                        string nv = str.Substring(0, n);
                        child.Value = nv;
                        
                        if (str != nv)
                        {
                            child.Children.Add(new CharTree(str));
                        }

                        if (cv != nv)
                        {
                            child.Children.Add(new CharTree(cv));
                            child.IsComplete = false;
                        }
                        
                        

                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    Children.Add(new CharTree(str));
                }
            }
            else
            {
                toAdd.Add(str);
            }
        }

        public CharTree? PrefixChild(string value)
        {
            foreach (CharTree child in Children)
            {
                if (value.StartsWith(child.Value))
                {
                    return child;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"({Value}, [{string.Join(", ", Children)}])";
        }
    }
}