using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Equation
    {
        private Node left, right;

        public Equation(Node left, Node right)
        {
            Right = right;
            Left = left;
        }

        public Node Right { get => right; set => right = value; }
        public Node Left { get => left; set => left = value; }

        public List<string> ContainedVariables => new Sum(Left, Right).ContainedVariables;
        public string Signature => "=";
    }
}
