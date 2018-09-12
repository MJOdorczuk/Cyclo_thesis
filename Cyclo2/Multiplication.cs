using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Multiplication : BiNode
    {
        public Multiplication(Node left, Node right) : base(left, right)
        {
            this.operation = (x, y) => x * y;
        }

        public override string Signature => " * ";

        public override bool IsCommutative => true;

        public override bool IsAssociative => true;

        public override Node Clone(Node left, Node right) => new Multiplication(left, right);
    }
}
