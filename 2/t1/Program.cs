using System;
using System.Collections.Generic;
using System.Linq;

namespace t1
{
    public class OnpExpression
    {
        private readonly char[] _exp;
        private readonly char[] _operators = {'+', '-', '*', '/', '%', '^'};
        private string _onp = "";

        public OnpExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            _exp = expression.ToCharArray(0, expression.Length);
            Calculate();
        }

        private void Calculate()
        {
            var stack = new Stack<char>();

            foreach (var c in _exp)
            {
                if (_operators.Contains(c))
                {
                    if (stack.Count > 0) _onp += stack.Pop();
                    stack.Push(c);
                }

                else _onp += c;
            }

            for (var i = 0; i < stack.Count; i++)
                _onp += stack.Pop();
        }

        public override string ToString() =>
            _onp;
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var onp1 = new OnpExpression("a - b * c");
            Console.WriteLine(onp1.ToString());

            var onp2 = new OnpExpression("a-b*c");
            Console.WriteLine(onp2.ToString());

            var onp3 = new OnpExpression("a+b*c/12+c");
            Console.WriteLine(onp3.ToString());
        }
    }
}